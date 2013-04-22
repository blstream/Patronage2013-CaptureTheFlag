/**
 * @author Milosz_Skalski
 * @author Rafal_Olichwer
 */
package com.blstream.ctf1;

import java.util.ArrayList;

import android.app.Activity;
import android.content.Intent;
import android.os.Bundle;
import android.view.View;
import android.view.View.OnClickListener;
import android.widget.Button;
import android.widget.EditText;

public class LoginActivity extends Activity implements OnClickListener {

	private Button mBtnLogin;
	private Button mBtnRegistration;
	private EditText mEditLogin;
	private EditText mEditPassword;
	
	@Override
	protected void onCreate(Bundle savedInstanceState) {
		
		
		super.onCreate(savedInstanceState);
		setContentView(R.layout.activity_login);
		
		mBtnLogin = (Button) findViewById(R.id.btnLogin);
		mBtnLogin.setOnClickListener(this);
		mBtnRegistration = (Button) findViewById(R.id.btnRegistration);
		mBtnRegistration.setOnClickListener(this);
		
		mEditLogin = (EditText) findViewById(R.id.editLogin);
		mEditPassword = (EditText) findViewById(R.id.editPassword);
	}

	@Override
	public void onClick(View v) {
		// TODO Auto-generated method stub
		Intent intent = null;

		switch(v.getId()) {
			case R.id.btnLogin:
				// instruction login
				String login = mEditLogin.getText().toString();
				String password = mEditPassword.getText().toString();
				ArrayList<String> nazwa = new ArrayList<String>();
				nazwa.get(0);
				
				break;
			case R.id.btnRegistration:
				intent = new Intent(this, RegisterActivity.class);
				startActivity(intent);
				break;
		}
	}
}
