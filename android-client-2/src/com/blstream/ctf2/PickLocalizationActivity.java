package com.blstream.ctf2;

import android.os.Bundle;

import com.google.android.maps.MapActivity;



/**
 * Pick Localization Activity
 * 
 * Activity displays Map with current position of the user;
 * User can pick localization for a game and define range of a game
 * 
 * @author Kamil Wisniewski
 */

public class PickLocalizationActivity extends MapActivity {

	
	
	@Override
	protected void onCreate(Bundle savedInstanceState) {
		// TODO Auto-generated method stub
		super.onCreate(savedInstanceState);
		setContentView(R.layout.activity_picklocalization);
		
		
	}

	@Override
	protected boolean isRouteDisplayed() {
		// TODO Auto-generated method stub
		return false;
	}

}