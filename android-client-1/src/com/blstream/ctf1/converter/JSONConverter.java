package com.blstream.ctf1.converter;

import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.Iterator;
import java.util.LinkedList;
import java.util.List;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import com.blstream.ctf1.Constants;
import com.blstream.ctf1.domain.GameBasicInfo;
import com.blstream.ctf1.domain.GameExtendedInfo;
import com.blstream.ctf1.domain.GameStatusType;
import com.blstream.ctf1.domain.Localization;
import com.google.android.gms.maps.model.LatLng;

/**
 * From JSON to any class
 * 
 * @author Adrian Swarcewicz
 */
public class JSONConverter {

	/**
	 * @author Adrian Swarcewicz
	 */
	public static List<String> toPlayerNameStrings(JSONArray jsonArrayPlayerNames) throws JSONException {
		List<String> result = new LinkedList<String>();
		for (int i = 0; i < jsonArrayPlayerNames.length(); i++) {
			String playerName = (String) jsonArrayPlayerNames.get(i);
			result.add(playerName);
		}
		return result;
	}

	/**
	 * @param jsonObject
	 * @return query string based on jsonObject or null if no keys-value pair
	 *         found in jsonObject
	 * @throws JSONException
	 * @author Adrian Swarcewicz
	 */
	public static String toQueryString(JSONObject jsonObject) throws JSONException {
		StringBuilder stringBuilder = new StringBuilder();
		Iterator<?> jsonIterator = jsonObject.keys();

		if (!jsonIterator.hasNext()) {
			return null;
		}

		while (jsonIterator.hasNext()) {
			String key = (String) jsonIterator.next();
			String value = jsonObject.get(key).toString();
			stringBuilder.append(key + "=" + StringConverter.urlEncode(value) + "&");
		}

		stringBuilder.setLength(stringBuilder.length() - 1);

		return stringBuilder.toString();
	}

	/**
	 * @param jsonArray
	 * @return
	 * @throws JSONException
	 * @author Adrian Swarcewicz
	 */
	public static List<GameBasicInfo> toGameBasicInfo(JSONArray jsonArray) throws JSONException {
		List<GameBasicInfo> gameBasicInfos = new LinkedList<GameBasicInfo>();

		for (int i = 0; i < jsonArray.length(); i++) {
			GameBasicInfo gameBasicInfo = new GameBasicInfo();
			JSONObject jo = jsonArray.getJSONObject(i);

			gameBasicInfo.setId(jo.getString("id"));
			gameBasicInfo.setName(jo.getString("name"));
			gameBasicInfo.setGameStatusType(GameStatusType.fromString(jo.getString("status")));
			gameBasicInfo.setOwner(jo.getString("owner"));

			gameBasicInfos.add(gameBasicInfo);
		}

		return gameBasicInfos;
	}

	/**
	 * @param jsonObject
	 * @return GameExtendedInfo
	 * @throws JSONException
	 * @author Rafał Olichwer
	 */
	public static GameExtendedInfo toGameExtendedInfo(JSONObject jsonObject) throws JSONException, ParseException {
		GameExtendedInfo result = new GameExtendedInfo();
		result.setDescription(jsonObject.getString("description"));
		result.setDuration(toMinutes(jsonObject.getLong("duration")));
		result.setGameStatusType(GameStatusType.fromString(jsonObject.getString("status")));
		JSONObject jsonLocalization = jsonObject.getJSONObject("localization");
		JSONArray jsonLatLng = jsonLocalization.getJSONArray("latLng");
		
		Localization localization = new Localization();
		localization.setName(jsonLocalization.getString("name"));
		localization.setRadius(jsonLocalization.getDouble("radius"));
		localization.setLatLng(new LatLng(jsonLatLng.getDouble(0),jsonLatLng.getDouble(1)));
		result.setLocalization(localization);
		result.setId(jsonObject.getString("id"));
		result.setName(jsonObject.getString("name"));
		result.setOwner(jsonObject.getString("owner"));
		result.setPlayersMax(jsonObject.getInt("players_max"));
		result.setPointsMax(jsonObject.getInt("points_max"));
		Date timeStart = new SimpleDateFormat(Constants.DATE_FORMAT + " " + Constants.TIME_FORMAT).parse(jsonObject.getString("time_start"));
		result.setTimeStart(timeStart);
		return result;
	}

	public static Long toMinutes(Long milis) {
		return milis / 60000;
	}
}