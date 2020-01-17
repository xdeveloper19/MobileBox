package mono.com.google.android.gms.drive.events;


public class CompletionListenerImplementor
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		com.google.android.gms.drive.events.CompletionListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onCompletion:(Lcom/google/android/gms/drive/events/CompletionEvent;)V:GetOnCompletion_Lcom_google_android_gms_drive_events_CompletionEvent_Handler:Android.Gms.Drive.Events.ICompletionListenerInvoker, Xamarin.GooglePlayServices.Drive\n" +
			"";
		mono.android.Runtime.register ("Android.Gms.Drive.Events.ICompletionListenerImplementor, Xamarin.GooglePlayServices.Drive", CompletionListenerImplementor.class, __md_methods);
	}


	public CompletionListenerImplementor ()
	{
		super ();
		if (getClass () == CompletionListenerImplementor.class)
			mono.android.TypeManager.Activate ("Android.Gms.Drive.Events.ICompletionListenerImplementor, Xamarin.GooglePlayServices.Drive", "", this, new java.lang.Object[] {  });
	}


	public void onCompletion (com.google.android.gms.drive.events.CompletionEvent p0)
	{
		n_onCompletion (p0);
	}

	private native void n_onCompletion (com.google.android.gms.drive.events.CompletionEvent p0);

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
