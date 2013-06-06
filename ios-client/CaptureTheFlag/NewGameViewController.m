//
//  NewGameViewController.m
//  CaptureTheFlag
//
//  Created by Konrad Gnoinski on 29.04.2013.
//  Copyright (c) 2013 BLStream. All rights reserved.
//

#import "NewGameViewController.h"
#import "CTFGame.h"
#import <AddressBookUI/AddressBookUI.h>


@interface NewGameViewController ()
@property (weak, nonatomic) IBOutlet UITextField *gameName;
@property (weak, nonatomic) IBOutlet UITextField *gameDescription;
@property (weak, nonatomic) IBOutlet UIDatePicker *gameStart;
@property (weak, nonatomic) IBOutlet UIDatePicker *gameDuration;
@property (weak, nonatomic) IBOutlet UIPickerView *gamePicker;
@property (weak, nonatomic) IBOutlet UITextField *gameRedName;
@property (weak, nonatomic) IBOutlet UITextField *gameBlueName;
@property (strong, nonatomic) NSString *addres;
@property (strong, nonatomic) CLLocation *gameLocation;
@property (strong, nonatomic) CLLocation *redTeamBaseLocalization;
@property (strong, nonatomic) CLLocation *blueTeamBaseLocalization;
@property (strong, nonatomic) NSNumber * gameRadius;

@end


@implementation NewGameViewController
int counter;

- (void)viewDidLoad
{
    [super viewDidLoad];
    //picker
    _gameStart.minimumDate = [NSDate date];
    _gamePicker.showsSelectionIndicator = TRUE;
    self.oneColumnList=[[NSArray alloc] initWithObjects:@"2",@"3",@"4",@"5",@"6",@"7",@"8",@"9",@"10",@"11",@"12", nil];
    self.secondColumnList=[[NSArray alloc] initWithObjects:@"5", @"10",@"15",@"20",@"25",@"30", @"40",@"50", nil];
   
    //map
    _mapView.showsUserLocation = YES;
    _mapView.delegate = self;
    self.geocoder = [[CLGeocoder alloc] init];
    
    //scrolling
    [Scroller setScrollEnabled:YES];
    [Scroller setContentSize:CGSizeMake(320, 1500)];
    //dissmising keybord after return
    UITapGestureRecognizer *tapRecognizer = [[UITapGestureRecognizer alloc] initWithTarget:self action:@selector(onTap:)];
    
    tapRecognizer.numberOfTapsRequired = 1;
    
    tapRecognizer.numberOfTouchesRequired = 1;
    
    [self.mapView addGestureRecognizer:tapRecognizer];
    UITapGestureRecognizer *tap = [[UITapGestureRecognizer alloc]
                                   initWithTarget:self
                                   action:@selector(dismissKeyboard)];
     [self.view addGestureRecognizer:tap];
    }

- (void)didReceiveMemoryWarning
{
    [super didReceiveMemoryWarning];
    // Dispose of any resources that can be recreated.
}

- (void)mapView:(MKMapView *)mapView1 regionDidChangeAnimated:(BOOL)animated
{
    if (![self.locationField.text isEqualToString:@""]) {
    [self.mapView removeOverlays:self.mapView.overlays];
    [self removeAllAnnotations];
    MKCoordinateSpan span = _mapView.region.span;
    CLLocationCoordinate2D centre = [_mapView centerCoordinate];
    MKCircle *circle = [MKCircle circleWithCenterCoordinate:centre radius:span.latitudeDelta*111*1000/2];
    self.gameRadius = [NSNumber numberWithDouble:(circle.radius)];
    [_mapView addOverlay:circle];
    
    MKPointAnnotation *annotationPoint = [[MKPointAnnotation alloc] init];
    annotationPoint.coordinate = centre;
    annotationPoint.title = @"placemark.name"; //??
    [_mapView addAnnotation:annotationPoint];
    MKCoordinateRegion region =
    MKCoordinateRegionMakeWithDistance(centre, span.latitudeDelta*111*1000, span.latitudeDelta*111*1000);
    [_mapView setRegion:region animated:YES];
    counter=0;
}
    else
{
    CLLocationCoordinate2D centerStartingCoordinates;
    centerStartingCoordinates.latitude=37.178181;
    centerStartingCoordinates.longitude=-96.054581;
    MKCoordinateRegion region =
    MKCoordinateRegionMakeWithDistance (
                                        centerStartingCoordinates , 2500000, 2500000);
    [_mapView setRegion:region animated:NO];
}
}

- (IBAction)geolocate:(id)sender
{
    [self.mapView removeOverlays:self.mapView.overlays];
    [self removeAllAnnotations];
    [self.geocoder geocodeAddressString:self.locationField.text
                      completionHandler:^(NSArray *coordinates, NSError
                                          *error) {
                          if (coordinates.count)
                          {
                              CLPlacemark *placemark = coordinates[0];
                              CLLocation *coordinate = placemark.location;
                              self.gameLocation = [[CLLocation alloc] initWithLatitude:coordinate.coordinate.latitude
                                                                        longitude:coordinate.coordinate.longitude];
                              MKCircle *circle = [MKCircle circleWithCenterCoordinate:coordinate.coordinate radius:450];
                              [_mapView addOverlay:circle];
                              
                              MKPointAnnotation *annotationPoint = [[MKPointAnnotation alloc] init];
                              annotationPoint.coordinate = coordinate.coordinate;
                              annotationPoint.title = @"placemark.name"; //??
                              [_mapView addAnnotation:annotationPoint];
                              MKCoordinateRegion region =
                              MKCoordinateRegionMakeWithDistance (
                                                                  coordinate.coordinate, 800, 800);
                              [_mapView setRegion:region animated:YES];
                              NSDictionary *addressDictionary = placemark.addressDictionary;
                              NSString* address =
                              ABCreateStringWithAddressDictionary(addressDictionary, NO);
                              self.addres = [address
                                         stringByReplacingOccurrencesOfString:@"\n" withString:@" "];
                          }
                          else
                          {
                              //error
                          }
                      }];
}

- (BOOL)textFieldShouldReturn:(UITextField *)textField {
    if (textField == self.gameName) {
        [textField resignFirstResponder];
        [self.gameDescription becomeFirstResponder];
    }
    else if (textField == self.gameDescription) {
        [textField resignFirstResponder];
        [self.locationField becomeFirstResponder];
    }
    else if (textField == self.locationField) {
        [textField resignFirstResponder];
        [self geolocate:(nil)];
    }
    else if (textField == self.gameRedName) {
        [textField resignFirstResponder];
        [self.gameBlueName becomeFirstResponder];
    }
    else if (textField == self.gameBlueName) {
        [textField resignFirstResponder];
        [self createNewGame:nil];
    }

    return YES;
}

-(void)dismissKeyboard {
    [_gameName resignFirstResponder];
    [_gameDescription resignFirstResponder];
    [_locationField resignFirstResponder];
    [_gameRedName resignFirstResponder];
    [_gameBlueName resignFirstResponder];
}

- (void)mapView:(MKMapView *)mapView
didUpdateUserLocation:
(MKUserLocation *)userLocation
{
   // _mapView.centerCoordinate =
   // userLocation.location.coordinate;
}

- (MKAnnotationView *) mapView:(MKMapView *)mapView viewForAnnotation:(id <MKAnnotation>) annotation
{
    if ([[annotation title] isEqualToString:@"Current Location"]) {
        return nil;
    }
    
    MKAnnotationView *annView = [[MKAnnotationView alloc ] initWithAnnotation:annotation reuseIdentifier:@"currentloc"];
    if ([[annotation title] isEqualToString:@"placemark.name"])
        annView.image = [ UIImage imageNamed:@"BLstream.png" ];
    else
        annView.image = [ UIImage imageNamed:@"pinRed.png" ];
    UIButton *infoButton = [UIButton buttonWithType:UIButtonTypeDetailDisclosure];
    [infoButton addTarget:self action:@selector(showDetailsView)
         forControlEvents:UIControlEventTouchUpInside];
    annView.canShowCallout = YES;
    return annView;
}

-(MKOverlayView *)mapView:(MKMapView *)mapView viewForOverlay:(id)overlay
{
    MKCircleView *circleView = [[MKCircleView alloc] initWithOverlay:overlay];
    circleView.strokeColor = [UIColor blueColor];
    circleView.lineWidth = 2;
    circleView.fillColor = [UIColor colorWithRed:0.0 green:0.0 blue:1.0 alpha:0.2];
    return circleView;
}

- (NSInteger)pickerView:(UIPickerView *)pickerView numberOfRowsInComponent:(NSInteger)component {
    if (component == 0) {
        return [_oneColumnList count];
    }
    
    return [_secondColumnList count];
}

- (NSInteger)numberOfComponentsInPickerView:(UIPickerView *)pickerView
{
    return 2;
}

- (NSString *)pickerView:(UIPickerView *)pickerView titleForRow:(NSInteger)row
            forComponent:(NSInteger)component
{
    if (component == 0) {
        return [_oneColumnList objectAtIndex:row];
        
    }
    
    return [_secondColumnList objectAtIndex:row];
    
}

- (void)pickerView:(UIPickerView *)pickerView didSelectRow:(NSInteger)row inComponent:(NSInteger)component
{
    if (component == 0) {
                return;
    }
    
    
}

-(void)removeAllAnnotations
{
    id userAnnotation = self.mapView.userLocation;
    
    NSMutableArray *annotations = [NSMutableArray arrayWithArray:self.mapView.annotations];
    [annotations removeObject:userAnnotation];
    
    [self.mapView removeAnnotations:annotations];
}


-(void)onTap:(UITapGestureRecognizer *)recognizer
{
    counter +=1;
    CGPoint point = [recognizer locationInView:self.mapView];
    
    CLLocationCoordinate2D coordinate = [self.mapView convertPoint:point toCoordinateFromView:self.mapView];
        
        if ((counter % 3)==1) {
            MKPointAnnotation *annotationPoint = [[MKPointAnnotation alloc] init];
            annotationPoint.coordinate = coordinate;
            annotationPoint.title = @"RED";
            [_mapView addAnnotation:annotationPoint];
        }
        
        if ((counter % 3)==2) {
            MKPointAnnotation *annotationPoint = [[MKPointAnnotation alloc] init];
            annotationPoint.coordinate = coordinate;
            annotationPoint.title = @"BLUE";
            [_mapView addAnnotation:annotationPoint];
        }
    if ((counter % 3)==0)
    {
        id userAnnotation = self.mapView.userLocation;
        NSMutableArray *annotations = [NSMutableArray arrayWithArray:self.mapView.annotations];
        [annotations removeObject:userAnnotation];
        [annotations removeObjectAtIndex:1];
        [self.mapView removeAnnotations:annotations];
   
    }
}
- (IBAction)createNewGame:(id)sender{
        CTFGame *myNewGame = [[CTFGame alloc] init];
        myNewGame.name = _gameName.text;
        myNewGame.gameDescription = _gameDescription.text;
        myNewGame.timeStart = _gameStart.date;
        NSNumber *durarionInMs = [NSNumber numberWithDouble:_gameDuration.countDownDuration];
        myNewGame.duration = durarionInMs;
        NSInteger row1, row2;
        NSString *pointsMaxFromPicker;
        NSString *playersMaxFromPicker;
        
        row1 = [_gamePicker selectedRowInComponent:0];
        row2 = [_gamePicker selectedRowInComponent:1];
        pointsMaxFromPicker = [_oneColumnList objectAtIndex:row1];
        playersMaxFromPicker = [_secondColumnList objectAtIndex:row2];
        myNewGame.pointsMax = [NSNumber numberWithInteger:[pointsMaxFromPicker integerValue]];
        myNewGame.playersMax = [NSNumber numberWithInteger:[playersMaxFromPicker integerValue]];
        myNewGame.localizationName = _addres;
        myNewGame.localization = _gameLocation;
        myNewGame.localizationRadius = _gameRadius;
        myNewGame.redTeamBaseName = _gameRedName.text;
        myNewGame.redTeamBaseLocalization = _redTeamBaseLocalization;
        myNewGame.blueTeamBaseName = _gameBlueName.text;
        myNewGame.blueTeamBaseLocalization =_blueTeamBaseLocalization;
   
    [[NetworkEngine getInstance] createNewGame:myNewGame completionBlock:^(NSObject *response){
        if ([response isKindOfClass:[NSError class]])
        {
            [ShowInformation showError:@"Failed to create a new game!"];
        }
        else
        {
            [ShowInformation showError:@"Game created sucesfully!"];
        }

    }];
}


@end
