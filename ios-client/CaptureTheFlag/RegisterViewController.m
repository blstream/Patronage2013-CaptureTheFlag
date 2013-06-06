//
//  ViewController.m
//  CaptureTheFlag
//
//  Created by Sebastian Jędruszkiewicz on 4/4/13.
//  Copyright (c) 2013 BLStream. All rights reserved.
//

#import "RegisterViewController.h"

static const CGFloat KEYBOARD_ANIMATION_DURATION = 0.3;
static const CGFloat MINIMUM_SCROLL_FRACTION = 0.2;
static const CGFloat MAXIMUM_SCROLL_FRACTION = 0.8;
static const CGFloat PORTRAIT_KEYBOARD_HEIGHT = 216;

@interface RegisterViewController ()
@property (weak, nonatomic) IBOutlet UITextField *userEmailField;
@property (weak, nonatomic) IBOutlet UITextField *passwordField;
@property (weak, nonatomic) IBOutlet UITextField *passwordField2;
@property (weak, nonatomic) IBOutlet UIView *emailBackgroundView;
@property (weak, nonatomic) IBOutlet UIView *passwordBackgroundView;
@property (weak, nonatomic) IBOutlet UIView *password2BackgroundView;
@property (weak, nonatomic) IBOutlet FlatButton *registerButton;
@property (weak, nonatomic) IBOutlet FlatButton *loginSwitchButton;
@property (weak, nonatomic) IBOutlet FlatButton *registerSwitchButton;
@property (weak, nonatomic) IBOutlet UIView *topBar;
@property (weak, nonatomic) IBOutlet UIView *emailIconBackgroundView;
@property (weak, nonatomic) IBOutlet UIView *passwordIconBackgroundView;
@property (weak, nonatomic) IBOutlet UIView *password2IconBackgroundView;
@property (readwrite) CGFloat animatedDistance;


@property (nonatomic, retain) UIAlertView *loginAlertView;

- (IBAction)reginster:(id)sender;
- (IBAction)goToLogin:(id)sender;

@end

@implementation RegisterViewController

- (IBAction)goToLogin:(id)sender{
    [self dismissViewControllerAnimated:YES completion:nil];
}

- (void)viewDidLoad
{
    [super viewDidLoad];
    UITapGestureRecognizer *tap = [[UITapGestureRecognizer alloc]
                                   initWithTarget:self
                                   action:@selector(dismissKeyboard)];
    
    [self.view addGestureRecognizer:tap];
    self.loginAlertView = [[UIAlertView alloc] initWithTitle:@"Please wait" message:nil
                                                    delegate:self
                                           cancelButtonTitle:nil
                                           otherButtonTitles:nil, nil];
    
    self.view.backgroundColor = [UIColor ctfApplicationBackgroundLighterColor];
    
    _emailBackgroundView.backgroundColor = [UIColor ctfInputBackgroundAndDisabledButtonColor];
    _passwordBackgroundView.backgroundColor = [UIColor ctfInputBackgroundAndDisabledButtonColor];
    _password2BackgroundView.backgroundColor = [UIColor ctfInputBackgroundAndDisabledButtonColor];
    
    _loginSwitchButton.buttonBackgroundColor = [UIColor ctfNormalButtonAndLabelTurquoiseColor];
    _loginSwitchButton.buttonHighlightedBackgroundColor = [UIColor ctfPressedButtonAndWindowBackgroundColor];
    
    _registerSwitchButton.buttonBackgroundColor = [UIColor ctfInputBackgroundAndDisabledButtonColor];
    _registerSwitchButton.buttonHighlightedBackgroundColor = [UIColor ctfPressedButtonAndWindowBackgroundColor];
    
    _registerButton.buttonBackgroundColor = [UIColor ctfNormalButtonAndLabelTurquoiseColor];
    _registerButton.buttonHighlightedBackgroundColor = [UIColor ctfPressedButtonAndWindowBackgroundColor];
    
    _emailIconBackgroundView.backgroundColor = [UIColor ctfNormalButtonAndLabelTurquoiseColor];
    _passwordIconBackgroundView.backgroundColor = [UIColor ctfNormalButtonAndLabelTurquoiseColor];
    _password2IconBackgroundView.backgroundColor = [UIColor ctfNormalButtonAndLabelTurquoiseColor];
    
    _topBar.backgroundColor = [UIColor ctfNormalButtonAndLabelCarrotColor];

	// Do any additional setup after loading the view, typically from a nib.
}

- (void)didReceiveMemoryWarning
{
    [super didReceiveMemoryWarning];
    // Dispose of any resources that can be recreated.
}
-(void)reginsterWithUserEmail:(NSString *)userEmail andPassword:(NSString *)password
{
    [[NetworkEngine getInstance] registerUser:userEmail
                             withPassword:(NSString *)password
                          completionBlock:^(NSObject *response) {
                              if ([response isKindOfClass:[NSError class]])
                              {
                                  [_loginAlertView dismissWithClickedButtonIndex:0 animated:YES];
                                  [ShowInformation showError:@"Failed to register new user"];
                              }
                              else
                              {
                                  [_loginAlertView dismissWithClickedButtonIndex:0 animated:YES];
                                  [ShowInformation showMessage:@"Your registration proces is complete" withTitle:nil];
                                  [self dismissViewControllerAnimated:YES completion:nil];
                            }
                          }];
}


- (IBAction)reginster:(id)sender{
            [_loginAlertView show];
if ([self.passwordField.text isEqualToString:self.passwordField2.text])
{
    [self beginReginster];
    [self reginsterWithUserEmail:self.userEmailField.text andPassword:self.passwordField.text];
}
else
{
    [_loginAlertView dismissWithClickedButtonIndex:0 animated:YES];
    [ShowInformation showError:@"passwords didn't match"];
}
}


-(void)beginReginster

{
    [self.userEmailField resignFirstResponder];
    [self.passwordField resignFirstResponder];
}

- (BOOL)textFieldShouldReturn:(UITextField *)textField {
    if (textField == self.userEmailField) {
        [textField resignFirstResponder];
        [self.passwordField becomeFirstResponder];
    }
        else if (textField == self.passwordField) {
            [textField resignFirstResponder];
            [self.passwordField2 becomeFirstResponder];
        }
        else if (textField == self.passwordField2) {
            [textField resignFirstResponder];
            [_loginAlertView show];
            if ([self.passwordField.text isEqualToString:self.passwordField2.text])
            {
                [self beginReginster];
                [self reginsterWithUserEmail:self.userEmailField.text andPassword:self.passwordField.text];
            }
            else
            {
                [_loginAlertView dismissWithClickedButtonIndex:0 animated:YES];
                [ShowInformation showError:@"passwords didn't match"];
            }

    }
    return YES;
    }

-(void)dismissKeyboard {
    [_userEmailField resignFirstResponder];
    [_passwordField resignFirstResponder];
    [_passwordField2 resignFirstResponder];
}

- (void)textFieldDidBeginEditing:(UITextField *)textField
{
    CGRect textFieldRect =
    [self.view.window convertRect:textField.bounds fromView:textField];
    CGRect viewRect =
    [self.view.window convertRect:self.view.bounds fromView:self.view];
    CGFloat midline = textFieldRect.origin.y + 0.5 * textFieldRect.size.height;
    CGFloat numerator =
    midline - viewRect.origin.y
    - MINIMUM_SCROLL_FRACTION * viewRect.size.height;
    CGFloat denominator =
    (MAXIMUM_SCROLL_FRACTION - MINIMUM_SCROLL_FRACTION)
    * viewRect.size.height;
    CGFloat heightFraction = numerator / denominator;if (heightFraction < 0.0)
    {
        heightFraction = 0.0;
    }
    else if (heightFraction > 1.0)
    {
        heightFraction = 1.0;
    }
    _animatedDistance = floor(PORTRAIT_KEYBOARD_HEIGHT * heightFraction);
    CGRect viewFrame = self.view.frame;
    viewFrame.origin.y -= _animatedDistance;
    
    [UIView beginAnimations:nil context:NULL];
    [UIView setAnimationBeginsFromCurrentState:YES];
    [UIView setAnimationDuration:KEYBOARD_ANIMATION_DURATION];
    
    [self.view setFrame:viewFrame];
    
    [UIView commitAnimations];
}

- (void)textFieldDidEndEditing:(UITextField *)textField
{
    CGRect viewFrame = self.view.frame;
    viewFrame.origin.y += _animatedDistance;
    
    [UIView beginAnimations:nil context:NULL];
    [UIView setAnimationBeginsFromCurrentState:YES];
    [UIView setAnimationDuration:KEYBOARD_ANIMATION_DURATION];
    
    [self.view setFrame:viewFrame];
    
    [UIView commitAnimations];
}


- (void)viewDidUnload {
    [self setEmailBackgroundView:nil];
    [self setPasswordBackgroundView:nil];
    [self setPassword2BackgroundView:nil];
    [self setRegisterButton:nil];
    [self setLoginSwitchButton:nil];
    [self setRegisterSwitchButton:nil];
    [self setTopBar:nil];
    [self setEmailIconBackgroundView:nil];
    [self setPasswordIconBackgroundView:nil];
    [self setPassword2IconBackgroundView:nil];
    [super viewDidUnload];
}


@end

