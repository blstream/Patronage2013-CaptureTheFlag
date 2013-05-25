package com.blstream.ctf1.list;
import android.util.Log;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ListView;
/**
 * 
 * @author Rafal Olichwer
 * class responsible to expand the listView to its max size,
 * so it won't be a scrolling element.
 *
 */
public class Helper {
    public static void getListViewSize(ListView myListView) {
        PlayersListAdapter myListAdapter = (PlayersListAdapter) myListView.getAdapter();
        if (myListAdapter == null) {
            return;
        }
        //set listAdapter in loop for getting final size
        int totalHeight = 0;
        for (int size = 0; size < myListAdapter.getCount(); size++) {
            View listItem = myListAdapter.getView(size, null, myListView);
            listItem.measure(0, 0);
            totalHeight += listItem.getMeasuredHeight();
        }
      //setting listview item in adapter
        ViewGroup.LayoutParams params = myListView.getLayoutParams();
        params.height = totalHeight + (myListView.getDividerHeight() * (myListAdapter.getCount() - 1));
        myListView.setLayoutParams(params);
        // print height of adapter on log
        Log.i("height of listItem:", String.valueOf(totalHeight));
    }
}
