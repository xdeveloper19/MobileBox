package crc64967e3bb99f80fa4c;


public class MainActivity_SamplePermissionListener
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		com.karumi.dexter.listener.multi.MultiplePermissionsListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onPermissionRationaleShouldBeShown:(Ljava/util/List;Lcom/karumi/dexter/PermissionToken;)V:GetOnPermissionRationaleShouldBeShown_Ljava_util_List_Lcom_karumi_dexter_PermissionToken_Handler:Com.Karumi.Dexter.Listener.Multi.IMultiplePermissionsListenerInvoker, EDMTBinding\n" +
			"n_onPermissionsChecked:(Lcom/karumi/dexter/MultiplePermissionsReport;)V:GetOnPermissionsChecked_Lcom_karumi_dexter_MultiplePermissionsReport_Handler:Com.Karumi.Dexter.Listener.Multi.IMultiplePermissionsListenerInvoker, EDMTBinding\n" +
			"";
		mono.android.Runtime.register ("GeoGeometry.Activity.MainActivity+SamplePermissionListener, GeoGeometry", MainActivity_SamplePermissionListener.class, __md_methods);
	}


	public MainActivity_SamplePermissionListener ()
	{
		super ();
		if (getClass () == MainActivity_SamplePermissionListener.class)
			mono.android.TypeManager.Activate ("GeoGeometry.Activity.MainActivity+SamplePermissionListener, GeoGeometry", "", this, new java.lang.Object[] {  });
	}

	public MainActivity_SamplePermissionListener (crc64967e3bb99f80fa4c.MainActivity p0)
	{
		super ();
		if (getClass () == MainActivity_SamplePermissionListener.class)
			mono.android.TypeManager.Activate ("GeoGeometry.Activity.MainActivity+SamplePermissionListener, GeoGeometry", "GeoGeometry.Activity.MainActivity, GeoGeometry", this, new java.lang.Object[] { p0 });
	}


	public void onPermissionRationaleShouldBeShown (java.util.List p0, com.karumi.dexter.PermissionToken p1)
	{
		n_onPermissionRationaleShouldBeShown (p0, p1);
	}

	private native void n_onPermissionRationaleShouldBeShown (java.util.List p0, com.karumi.dexter.PermissionToken p1);


	public void onPermissionsChecked (com.karumi.dexter.MultiplePermissionsReport p0)
	{
		n_onPermissionsChecked (p0);
	}

	private native void n_onPermissionsChecked (com.karumi.dexter.MultiplePermissionsReport p0);

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
