package com.ethanzarov.cloud;

import android.app.Activity;
import android.content.Context;
import android.content.SharedPreferences;
import android.util.Log;
import com.unity3d.player.UnityPlayer;

import com.google.android.gms.auth.api.signin.GoogleSignIn;
import com.google.android.gms.auth.api.signin.GoogleSignInAccount;
import com.google.android.gms.auth.api.signin.GoogleSignInClient;
import com.google.android.gms.auth.api.signin.GoogleSignInOptions;
import com.google.android.gms.games.Games;
import com.google.android.gms.games.SnapshotsClient;
import com.google.android.gms.games.snapshot.Snapshot;
import com.google.android.gms.games.snapshot.SnapshotMetadata;
import com.google.android.gms.games.snapshot.SnapshotMetadataChange;
import com.google.android.gms.tasks.Task;
import com.google.android.gms.tasks.OnSuccessListener;
import com.google.android.gms.tasks.OnFailureListener;

import org.json.JSONObject;
import org.json.JSONException;

import java.io.IOException;
import java.util.Calendar;

public class EZCloud {
    private static final String TAG = "default";
    private static final String CLOUD_SAVE_PREFIX = "EZCloud";
    private static final int MAX_SNAPSHOT_RESOLVE_RETRIES = 3;
    
    private String gameIdentifier = "";
    private String cloudSaveName = "";
    
    public Context ctx;
    
    private SharedPreferences prefs;
    private SharedPreferences.Editor prefsEditor;
    
    private GoogleSignInClient googleSignInClient;
    private GoogleSignInAccount signedInAccount;
    private SnapshotsClient snapshotsClient;
    private Snapshot currentSnapshot;
    private String currentSnapshotContent;
    private boolean isCloudInitialized = false;
    
    private static String unityCallbackObject = "";
    private static String unityCallbackMethod = "";
    

    public static void setUnityLoadFromCloudCallback(String gameObjectName, String methodName) {
        unityCallbackObject = gameObjectName;
        unityCallbackMethod = methodName;
        Log.d(TAG, "Set Unity callback to: " + gameObjectName + "." + methodName);
    }


    public void setGameIdentifier(String identifier) {
        this.gameIdentifier = identifier;
        this.cloudSaveName = CLOUD_SAVE_PREFIX + identifier;
        Log.d(TAG, "Game identifier set to: " + identifier + ", cloud save name: " + cloudSaveName);
    }
    

    private String getGameSpecificKey(String key) {
        if (gameIdentifier == null || gameIdentifier.isEmpty()) {
            // If no game ID set, use package name as fallback
            if (ctx != null) {
                gameIdentifier = ctx.getPackageName();
            } else {
                gameIdentifier = "unknown";
            }
        }
        return gameIdentifier + "." + key;
    }
    

    public void initialize() {
        // Initialize local SharedPreferences storage
        prefs = ctx.getSharedPreferences("localData_" + gameIdentifier, Context.MODE_PRIVATE);
        prefsEditor = prefs.edit();
        
        // Set default game identifier if not already set
        if (gameIdentifier == null || gameIdentifier.isEmpty()) {
            setGameIdentifier(ctx.getPackageName());
        }
        
        try {
            GoogleSignInOptions signInOptions = new GoogleSignInOptions.Builder(GoogleSignInOptions.DEFAULT_GAMES_SIGN_IN)
                    .build();
            
            googleSignInClient = GoogleSignIn.getClient(ctx, signInOptions);
            
            signedInAccount = GoogleSignIn.getLastSignedInAccount(ctx);
            if (signedInAccount != null) {
                onConnected(signedInAccount);
            } else {
                googleSignInClient.silentSignIn().addOnSuccessListener((Activity) ctx, 
                    new OnSuccessListener<GoogleSignInAccount>() {
                        @Override
                        public void onSuccess(GoogleSignInAccount account) {
                            signedInAccount = account;
                            onConnected(account);
                        }
                    }
                ).addOnFailureListener((Activity) ctx,
                    new OnFailureListener() {
                        @Override
                        public void onFailure(Exception e) {

                        }
                    }
                );
            }
        } catch (Exception e) {
            Log.e(TAG, "Error initializing cloud save: " + e.getMessage());
        }
    }
    
    /**
     * Called when connected to Google Play Games
     */
    private void onConnected(GoogleSignInAccount account) {
        snapshotsClient = Games.getSnapshotsClient(ctx, account);
        isCloudInitialized = true;
        Log.d(TAG, "Connected to Google Play Games Services");
        
        // Load snapshot data
        loadFromCloud();
    }
    
    /**
     * Check if cloud save is available
     */
    public boolean isCloudAvailable() {
        return isCloudInitialized && signedInAccount != null;
    }
    
    /**
     * Save string to local storage
     */
    public void saveString(String key, String valueToSave) {
        prefsEditor.putString(getGameSpecificKey(key), valueToSave);
        prefsEditor.apply();
    }
    
    /**
     * Load string from local storage
     */
    public String loadString(String key) {
        return prefs.getString(getGameSpecificKey(key), "");
    }
    
    /**
     * Save boolean to local storage
     */
    public void saveBool(String key, boolean valueToSave) {
        prefsEditor.putBoolean(getGameSpecificKey(key), valueToSave);
        prefsEditor.apply();
    }
    
    /**
     * Load boolean from local storage
     */
    public boolean loadBool(String key) {
        return prefs.getBoolean(getGameSpecificKey(key), false);
    }
    
    /**
     * Save integer to local storage
     */
    public void saveInt(String key, int valueToSave) {
        prefsEditor.putInt(getGameSpecificKey(key), valueToSave);
        prefsEditor.apply();
    }
    
    /**
     * Load integer from local storage
     */
    public int loadInt(String key) {
        return prefs.getInt(getGameSpecificKey(key), 0);
    }
    
    /**
     * Save long to local storage
     */
    public void saveLong(String key, long valueToSave) {
        prefsEditor.putLong(getGameSpecificKey(key), valueToSave);
        prefsEditor.apply();
    }
    
    /**
     * Load long from local storage
     */
    public long loadLong(String key) {
        return prefs.getLong(getGameSpecificKey(key), 0);
    }
    
    /**
     * Save float to local storage
     */
    public void saveFloat(String key, float valueToSave) {
        prefsEditor.putFloat(getGameSpecificKey(key), valueToSave);
        prefsEditor.apply();
    }
    
    /**
     * Load float from local storage
     */
    public float loadFloat(String key) {
        return prefs.getFloat(getGameSpecificKey(key), 0.0f);
    }
    
    /**
     * Check if key exists in local storage
     */
    public boolean keyExists(String key) {
        return prefs.contains(getGameSpecificKey(key));
    }
    
    /**
     * Load data as JSON string
     */
    public String loadNativeDataAsJSONString(String key) {
        try {
            SharedPreferences.Editor tempEdit = prefs.edit();
            tempEdit.apply();
            
            JSONObject all = new JSONObject();
            for (String storedKey : prefs.getAll().keySet()) {
                // Only include keys for this game
                String prefix = gameIdentifier + ".";
                if (storedKey.startsWith(prefix)) {
                    // Remove the prefix to get the original key
                    String originalKey = storedKey.substring(prefix.length());
                    all.put(originalKey, prefs.getAll().get(storedKey));
                }
            }
            
            if (key != null && !key.isEmpty()) {
                // If a specific key was requested
                String gameKey = getGameSpecificKey(key);
                if (prefs.contains(gameKey)) {
                    JSONObject single = new JSONObject();
                    single.put(key, prefs.getAll().get(gameKey));
                    return single.toString();
                } else {
                    return "{\"err_msg\":\"Key not found\"}";
                }
            } else {
                // Return all game-specific keys
                return all.toString();
            }
        } catch (JSONException e) {
            return "{\"err_msg\":\"" + e.getMessage() + "\"}";
        }
    }
    
    /**
     * Sync local changes to cloud
     */
    public void syncToCloud() {
        if (!isCloudAvailable()) {
            Log.w(TAG, "Cannot sync to cloud - cloud not available");
            return;
        }
        
        try {
            // Create JSON containing only game-specific keys
            final JSONObject saveData = new JSONObject();
            for (String key : prefs.getAll().keySet()) {
                String prefix = gameIdentifier + ".";
                if (key.startsWith(prefix)) {
                    // Remove the prefix to get the original key
                    String originalKey = key.substring(prefix.length());
                    saveData.put(originalKey, prefs.getAll().get(key));
                }
            }
            
            // Open the saved game
            snapshotsClient.open(cloudSaveName, true)
                .addOnSuccessListener((Activity) ctx, result -> {
                    try {
                        // Handle data based on response type
                        if (result.isConflict()) {
                            // Handle conflict resolution
                            handleSnapshotConflict(result, saveData);
                        } else {
                            currentSnapshot = result.getData();
                            writeSnapshotData(currentSnapshot, saveData);
                        }
                    } catch (Exception e) {
                        Log.e(TAG, "Error syncing to cloud: " + e.getMessage());
                    }
                })
                .addOnFailureListener((Activity) ctx,
                    new OnFailureListener() {
                        @Override
                        public void onFailure(Exception e) {
                            Log.e(TAG, "Failed to open snapshot: " + e.getMessage());
                        }
                    }
                );
        } catch (Exception e) {
            Log.e(TAG, "Error syncing to cloud: " + e.getMessage());
        }
    }
    
    /**
     * Write data to a snapshot and commit changes
     */
    private void writeSnapshotData(Snapshot snapshot, JSONObject data) {
        try {
            // Write the game data to the snapshot
            snapshot.getSnapshotContents().writeBytes(data.toString().getBytes());
            
            // Create the change metadata
            SnapshotMetadataChange metadataChange = new SnapshotMetadataChange.Builder()
                .setDescription("Save at " + Calendar.getInstance().getTime().toString())
                .build();
            
            // Commit the change
            snapshotsClient.commitAndClose(snapshot, metadataChange)
                .addOnSuccessListener((Activity) ctx, 
                    new OnSuccessListener<SnapshotMetadata>() {
                        @Override
                        public void onSuccess(SnapshotMetadata snapshotMetadata) {
                            Log.d(TAG, "Saved game synced to cloud: " + cloudSaveName);
                        }
                    }
                )
                .addOnFailureListener((Activity) ctx,
                    new OnFailureListener() {
                        @Override
                        public void onFailure(Exception e) {
                            Log.e(TAG, "Failed to commit snapshot: " + e.getMessage());
                        }
                    }
                );
        } catch (Exception e) {
            Log.e(TAG, "Error writing snapshot data: " + e.getMessage());
        }
    }
    
    /**
     * Load data from cloud to local storage
     */
    public void loadFromCloud() {
        if (!isCloudAvailable()) {
            Log.w(TAG, "Cannot load from cloud - cloud not available");
            sendUnityCallback(false);
            return;
        }
        
        try {
            // Open the saved game
            snapshotsClient.open(cloudSaveName, true)
                .addOnSuccessListener((Activity) ctx, result -> {
                    try {
                        // Handle data based on response type
                        if (result.isConflict()) {
                            // Handle conflict resolution
                            handleConflictForLoad(result);
                        } else {
                            currentSnapshot = result.getData();
                            readSnapshotData(currentSnapshot);
                        }
                    } catch (Exception e) {
                        Log.e(TAG, "Error processing snapshot: " + e.getMessage());
                        sendUnityCallback(false);
                    }
                })
                .addOnFailureListener((Activity) ctx,
                    new OnFailureListener() {
                        @Override
                        public void onFailure(Exception e) {
                            Log.e(TAG, "Failed to open snapshot: " + e.getMessage());
                            sendUnityCallback(false);
                        }
                    }
                );
        } catch (Exception e) {
            Log.e(TAG, "Error loading from cloud: " + e.getMessage());
            sendUnityCallback(false);
        }
    }
    
    /**
     * Read data from a snapshot
     */
    private void readSnapshotData(Snapshot snapshot) {
        try {
            byte[] data = snapshot.getSnapshotContents().readFully();
            if (data != null && data.length > 0) {
                String json = new String(data);
                currentSnapshotContent = json;
                
                // Parse JSON and apply to local storage
                JSONObject cloudData = new JSONObject(json);
                
                // Remove existing game-specific keys
                SharedPreferences.Editor tempEditor = prefs.edit();
                String prefix = gameIdentifier + ".";
                for (String key : prefs.getAll().keySet()) {
                    if (key.startsWith(prefix)) {
                        tempEditor.remove(key);
                    }
                }
                tempEditor.apply();
                
                // Load new data from cloud (original keys without prefix in the cloud)
                java.util.Iterator<String> keys = cloudData.keys();
                while (keys.hasNext()) {
                    String key = keys.next();
                    Object value = cloudData.get(key);
                    String gameKey = getGameSpecificKey(key);
                    
                    if (value instanceof String) {
                        prefsEditor.putString(gameKey, (String) value);
                    } else if (value instanceof Boolean) {
                        prefsEditor.putBoolean(gameKey, (Boolean) value);
                    } else if (value instanceof Integer) {
                        prefsEditor.putInt(gameKey, (Integer) value);
                    } else if (value instanceof Long) {
                        prefsEditor.putLong(gameKey, (Long) value);
                    } else if (value instanceof Float) {
                        prefsEditor.putFloat(gameKey, (Float) value);
                    } else if (value instanceof Double) {
                        // Store doubles as strings to preserve precision
                        prefsEditor.putString(gameKey, value.toString());
                    }
                }
                
                prefsEditor.apply();
                Log.d(TAG, "Successfully loaded cloud data for " + cloudSaveName);
                sendUnityCallback(true);
            } else {
                Log.w(TAG, "No data found in snapshot");
                sendUnityCallback(false);
            }
        } catch (IOException e) {
            Log.e(TAG, "Error reading snapshot data: " + e.getMessage());
            sendUnityCallback(false);
        } catch (Exception e) {
            Log.e(TAG, "Error processing snapshot data: " + e.getMessage());
            sendUnityCallback(false);
        }
    }
    
    /**
     * Handle snapshot conflicts for loading
     */
    private void handleConflictForLoad(SnapshotsClient.DataOrConflict<Snapshot> result) {
        try {
            SnapshotsClient.SnapshotConflict conflict = result.getConflict();
            if (conflict != null) {
                Snapshot snapshot1 = conflict.getSnapshot();
                Snapshot snapshot2 = conflict.getConflictingSnapshot();
                
                // Resolve conflict by selecting the newer one
                long timestamp1 = snapshot1.getMetadata().getLastModifiedTimestamp();
                long timestamp2 = snapshot2.getMetadata().getLastModifiedTimestamp();
                
                Snapshot resolvedSnapshot = (timestamp1 > timestamp2) ? snapshot1 : snapshot2;
                
                // Resolve the conflict
                snapshotsClient.resolveConflict(conflict.getConflictId(), resolvedSnapshot)
                    .addOnSuccessListener((Activity) ctx, newResult -> {
                        // Process the resolved snapshot
                        if (newResult.isConflict()) {
                            // If still conflict, give up after a few tries
                            Log.e(TAG, "Unable to resolve conflict after attempt");
                            sendUnityCallback(false);
                        } else {
                            // Successfully resolved
                            readSnapshotData(newResult.getData());
                        }
                    })
                    .addOnFailureListener((Activity) ctx, e -> {
                        Log.e(TAG, "Failed to resolve conflict: " + e.getMessage());
                        sendUnityCallback(false);
                    });
            } else {
                Log.e(TAG, "Conflict marked but no conflict data found");
                sendUnityCallback(false);
            }
        } catch (Exception e) {
            Log.e(TAG, "Error handling snapshot conflict: " + e.getMessage());
            sendUnityCallback(false);
        }
    }
    
    /**
     * Handle snapshot conflicts for saving
     */
    private void handleSnapshotConflict(SnapshotsClient.DataOrConflict<Snapshot> result, JSONObject saveData) {
        try {
            SnapshotsClient.SnapshotConflict conflict = result.getConflict();
            if (conflict != null) {
                Snapshot snapshot1 = conflict.getSnapshot();
                Snapshot snapshot2 = conflict.getConflictingSnapshot();
                
                // Resolve conflict by selecting the newer one
                long timestamp1 = snapshot1.getMetadata().getLastModifiedTimestamp();
                long timestamp2 = snapshot2.getMetadata().getLastModifiedTimestamp();
                
                Snapshot resolvedSnapshot = (timestamp1 > timestamp2) ? snapshot1 : snapshot2;
                
                // Resolve the conflict
                snapshotsClient.resolveConflict(conflict.getConflictId(), resolvedSnapshot)
                    .addOnSuccessListener((Activity) ctx, newResult -> {
                        // Process the resolved snapshot
                        if (newResult.isConflict()) {
                            // If still conflict, give up after a few tries
                            Log.e(TAG, "Unable to resolve conflict after attempt");
                        } else {
                            // Successfully resolved
                            writeSnapshotData(newResult.getData(), saveData);
                        }
                    })
                    .addOnFailureListener((Activity) ctx, e -> {
                        Log.e(TAG, "Failed to resolve conflict: " + e.getMessage());
                    });
            } else {
                Log.e(TAG, "Conflict marked but no conflict data found");
            }
        } catch (Exception e) {
            Log.e(TAG, "Error handling snapshot conflict: " + e.getMessage());
        }
    }
    
    /**
     * Send callback to Unity
     */
    private void sendUnityCallback(boolean success) {
        if (!unityCallbackObject.isEmpty() && !unityCallbackMethod.isEmpty()) {
            try {
                JSONObject result = new JSONObject();
                result.put("success", success);
                UnityPlayer.UnitySendMessage(unityCallbackObject, unityCallbackMethod, result.toString());
            } catch (Exception e) {
                Log.e(TAG, "Error sending Unity callback: " + e.getMessage());
            }
        }
    }
}