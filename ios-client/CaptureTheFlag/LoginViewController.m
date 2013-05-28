//
//  MainViewController.m
//  CaptureTheFlag
//
//  Created by Konrad Gnoinski on 12.04.2013.
//  Copyright (c) 2013 BLStream. All rights reserved.
//


 #import "LoginViewController.h"


@interface LoginViewController ()

@property (weak, nonatomic) IBOutlet UITextField *userEmailField;
@property (weak, nonatomic) IBOutlet UITextField *passwordField;
@property (weak, nonatomic) IBOutlet UIView *emailBackgroundView;
@property (weak, nonatomic) IBOutlet UIView *passwordBackgroundView;
@property (weak, nonatomic) IBOutlet FlatButton *loginButton;
@property (weak, nonatomic) IBOutlet FlatButton *loginSwitchButton;
@property (weak, nonatomic) IBOutlet FlatButton *registerSwitchButton;
@property (weak, nonatomic) IBOutlet UIView *topBar;
@property (weak, nonatomic) IBOutlet UIView *emailIconBackgroundView;
@property (weak, nonatomic) IBOutlet UIView *passwordIconBackgroundView;

- (IBAction)login:(id)sender;

@property (nonatomic, retain) UIAlertView *loginAlertView;

@end

@implementation LoginViewController

- (void)viewDidLoad
{
    [super viewDidLoad];
    UITapGestureRecognizer *tap = [[UITapGestureRecognizer alloc]
                                   initWithTarget:self
                                   action:@selector(dismissKeyboard)];
    
    [self.view addGestureRecognizer:tap];
	// Do any additional setup after loading the view, typically from a nib.
    self.loginAlertView = [[UIAlertView alloc] initWithTitle:@"Please wait" message:nil
                                                       delegate:self
                                              cancelButtonTitle:nil
                                              otherButtonTitles:nil, nil];
 
    self.view.backgroundColor = [UIColor ctfApplicationBackgroundLighterColor];
    
    _emailBackgroundView.backgroundColor = [UIColor ctfInputBackgroundAndDisabledButtonColor];
    _passwordBackgroundView.backgroundColor = [UIColor ctfInputBackgroundAndDisabledButtonColor];
    
    _loginSwitchButton.buttonBackgroundColor = [UIColor ctfNormalButtonAndLabelTurquoiseColor];
    _loginSwitchButton.buttonHighlightedBackgroundColor = [UIColor ctfPressedButtonAndWindowBackgroundColor];

    _registerSwitchButton.buttonBackgroundColor = [UIColor ctfInputBackgroundAndDisabledButtonColor];
    _registerSwitchButton.buttonHighlightedBackgroundColor = [UIColor ctfPressedButtonAndWindowBackgroundColor];

    _loginButton.buttonBackgroundColor = [UIColor ctfNormalButtonAndLabelTurquoiseColor];
    _loginButton.buttonHighlightedBackgroundColor = [UIColor ctfPressedButtonAndWindowBackgroundColor];
    
    _emailIconBackgroundView.backgroundColor = [UIColor ctfNormalButtonAndLabelTurquoiseColor];
    _passwordIconBackgroundView.backgroundColor = [UIColor ctfNormalButtonAndLabelTurquoiseColor];
    
    _topBar.backgroundColor = [UIColor ctfNormalButtonAndLabelCarrotColor];
    
}


- (void)didReceiveMemoryWarning
{
    [super didReceiveMemoryWarning];
    // Dispose of any resources that can be recreated.
}


-(void)loginWithUserEmail:(NSString *)userEmail andPassword:(NSString *)password
{
    [[NetworkEngine getInstance]
                                 loginWithEmail:userEmail
                                 withPassword:(NSString *)password
                                 completionBlock:^(NSObject *response) {
                                  if ([response isKindOfClass:[NSError class]])
                                  {
                                      [_loginAlertView dismissWithClickedButtonIndex:0 animated:YES];
                                      [ShowInformation showError:@"Incorrect user name or password"];                                  }
                                  else
                                  {
                                      [self performSegueWithIdentifier:@"segueToMainScreenAfterLogin" sender:self];
                                      [_loginAlertView dismissWithClickedButtonIndex:0 animated:YES];
                                  }
                              }];
}


- (IBAction)login:(id)sender{
        [self beginLogin];
        [self loginWithUserEmail:self.userEmailField.text andPassword:self.passwordField.text];
    [_loginAlertView show];
}

-(void)beginLogin

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
        [self beginLogin];
        [self loginWithUserEmail:self.userEmailField.text andPassword:self.passwordField.text];
        [_loginAlertView show];
    }
    return YES;
}

-(void)dismissKeyboard {
    [_userEmailField resignFirstResponder];
    [_passwordField resignFirstResponder];
}

- (void)viewDidUnload {
    [self setEmailBackgroundView:nil];
    [self setPasswordBackgroundView:nil];
    [self setLoginButton:nil];
    [self setLoginSwitchButton:nil];
    [self setRegisterSwitchButton:nil];
    [self setTopBar:nil];
    [self setEmailIconBackgroundView:nil];
    [self setPasswordIconBackgroundView:nil];
    [super viewDidUnload];
}
@end
