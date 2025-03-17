#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>

NS_ASSUME_NONNULL_BEGIN

@interface EZHaptics : NSObject

+ (void)performTransientHaptic:(float)intensity sharpness:(float)sharpness;

@end

NS_ASSUME_NONNULL_END
