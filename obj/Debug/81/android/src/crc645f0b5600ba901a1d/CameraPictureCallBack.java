package crc645f0b5600ba901a1d;


public class CameraPictureCallBack
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("GeoGeometry.Activity.Cameraa.CameraPictureCallBack, GeoGeometry", CameraPictureCallBack.class, __md_methods);
	}


	public CameraPictureCallBack ()
	{
		super ();
		if (getClass () == CameraPictureCallBack.class)
			mono.android.TypeManager.Activate ("GeoGeometry.Activity.Cameraa.CameraPictureCallBack, GeoGeometry", "", this, new java.lang.Object[] {  });
	}

	public CameraPictureCallBack (android.content.Context p0)
	{
		super ();
		if (getClass () == CameraPictureCallBack.class)
			mono.android.TypeManager.Activate ("GeoGeometry.Activity.Cameraa.CameraPictureCallBack, GeoGeometry", "Android.Content.Context, Mono.Android", this, new java.lang.Object[] { p0 });
	}

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
