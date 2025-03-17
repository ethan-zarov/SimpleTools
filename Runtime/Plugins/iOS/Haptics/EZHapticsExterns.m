#import "EZHaptics.h"


void performTransientHaptic(float intensity, float sharpness)
{
    [TOSNativeHapticsIos performTransientHaptic:intensity
                                      sharpness:sharpness];
}
