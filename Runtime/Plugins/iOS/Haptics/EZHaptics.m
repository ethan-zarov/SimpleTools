#import "EZHaptics.h"
#import <AudioToolbox/AudioToolbox.h>
#import <CoreHaptics/CoreHaptics.h>

@implementation EZHaptics

+ (void)performTransientHaptic:(float)intensity sharpness:(float)sharpness
{
    if (@available(iOS 13.0, *)) {
        static CHHapticEngine *engine = nil;
    
        if (!engine) {
            NSError *error = nil;
            engine = [[CHHapticEngine alloc] initAndReturnError:&error];
            if (!error) {
                engine.stoppedHandler = ^(CHHapticEngineStoppedReason reason){
                    NSLog(@"Haptic engine stopped: %ld", (long)reason);
                    engine = nil;
                };
                
                engine.resetHandler = ^{
                    NSLog(@"Haptic engine reset");
                    NSError *startError = nil;
                    [engine startAndReturnError:&startError];
                };
                
                [engine startAndReturnError:&error];
            }
        }
        
        CHHapticEventParameter *intensityParam =
            [[CHHapticEventParameter alloc] initWithParameterID:CHHapticEventParameterIDHapticIntensity
                                                         value:intensity];
        CHHapticEventParameter *sharpnessParam =
            [[CHHapticEventParameter alloc] initWithParameterID:CHHapticEventParameterIDHapticSharpness
                                                         value:sharpness];

        CHHapticEvent *transientEvent =
        [[CHHapticEvent alloc] initWithEventType:CHHapticEventTypeHapticTransient
                                           parameters:@[intensityParam, sharpnessParam]
                                          relativeTime:0.0];

        NSError *error = nil;
        CHHapticPattern *pattern =
            [[CHHapticPattern alloc] initWithEvents:@[transientEvent] parameters:@[] error:&error];

        // Create a player and start the haptic
        id<CHHapticPatternPlayer> player = [engine createPlayerWithPattern:pattern error:&error];
        [player startAtTime:0 error:&error];
    }
    else {
        // Fallback for older iOS versions
        // e.g., approximate to existing ImpactFeedback styles
        // For instance, map intensity to Light/Medium/Heavy:
        if (intensity < 0.33) {
            UIImpactFeedbackGenerator *lightGen =
              [[UIImpactFeedbackGenerator alloc] initWithStyle:UIImpactFeedbackStyleLight];
            [lightGen impactOccurred];
        } else if (intensity < 0.66) {
            UIImpactFeedbackGenerator *mediumGen =
              [[UIImpactFeedbackGenerator alloc] initWithStyle:UIImpactFeedbackStyleMedium];
            [mediumGen impactOccurred];
        } else {
            UIImpactFeedbackGenerator *heavyGen =
              [[UIImpactFeedbackGenerator alloc] initWithStyle:UIImpactFeedbackStyleHeavy];
            [heavyGen impactOccurred];
        }
    }
}

@end
