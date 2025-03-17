import android.app.Activity;
import android.content.Context;
import android.os.Build;
import android.os.Handler;
import android.os.VibrationEffect;
import android.os.Vibrator;
import android.util.Log;
import android.view.HapticFeedbackConstants;
import android.view.View;

class EZHaptics {

    public Context ctx;
    
   public void playHapticTransient(float intensity, float sharpness) {
        // 1) Get Vibrator service from the Android context
        Vibrator vibrator = (Vibrator) ctx.getSystemService(Context.VIBRATOR_SERVICE);
        if (vibrator == null) {
            Log.e("EZHaptics", "No vibration service available.");
            return;
        }

        if (intensity < 0) intensity = 0;
        if (intensity > 1) intensity = 1;
        int amplitude = (int) (intensity * 255f);
        if (amplitude < 1) amplitude = 1;  // 0 amplitude = silent

        if (sharpness < 0) sharpness = 0;
        if (sharpness > 1) sharpness = 1;
        long durationMs = (long) (20 + 80 * sharpness);  // range ~30..100 ms

        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.O) {
            VibrationEffect effect = VibrationEffect.createOneShot(durationMs, amplitude);
            vibrator.vibrate(effect);
        } else {
            vibrator.vibrate(durationMs);
        }
    }

}