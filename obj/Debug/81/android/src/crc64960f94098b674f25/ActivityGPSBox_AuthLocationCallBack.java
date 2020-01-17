package crc64960f94098b674f25;


public class ActivityGPSBox_AuthLocationCallBack
	extends crc64e95e69e34d869711.LocationCallback
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onLocationResult:(Lcom/google/android/gms/location/LocationResult;)V:GetOnLocationResult_Lcom_google_android_gms_location_LocationResult_Handler\n" +
			"";
		mono.android.Runtime.register ("GeoGeometry.Activity.Auth.ActivityGPSBox+AuthLocationCallBack, GeoGeometry", ActivityGPSBox_AuthLocationCallBack.class, __md_methods);
	}


	public ActivityGPSBox_AuthLocationCallBack ()
	{
		super ();
		if (getClass () == ActivityGPSBox_AuthLocationCallBack.class)
			mono.android.TypeManager.Activate ("GeoGeometry.Activity.Auth.ActivityGPSBox+AuthLocationCallBack, GeoGeometry", "", this, new java.lang.Object[] {  });
	}


	public void onLocationResult (com.google.android.gms.location.LocationResult p0)
	{
		n_onLocationResult (p0);
	}

	private native void n_onLocationResult (com.google.android.gms.location.LocationResult p0);

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
