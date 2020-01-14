using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;

namespace GeoGeometry.Activity.Auth
{
    [Activity(Label = "SensorsDataActivity")]
    public class SensorsDataActivity: AppCompatActivity
    {
        TableLayout tablelayout;
        //public List<Box> mItems;
        TextView productname, price;
        private ImageButton btn_back_a1;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_sensors);

            //List<UserCartModel> mItems = await GetData();
            tablelayout = FindViewById<TableLayout>(Resource.Id.sensors_data);
            btn_back_a1 = FindViewById<ImageButton>(Resource.Id.btn_back_a1);
            //BindData();

            btn_back_a1.Click += (s, e) =>
            {
                Intent Driver = new Intent(this, typeof(Auth.DriverActivity));
                Driver.PutExtra("idAction", "2");// через объект идёт обращение к . 
                StartActivity(Driver);
            };
        }


        //public async Task<List<UserCartModel>> GetData()
        //{
        //    HttpClient client = new HttpClient();
        //    string ac = Common.Url;
        //    string url = ac + "RestaurentOderList?id=" + sessionid;

        //    var request = HttpWebRequest.Create(url);
        //    request.Method = "GET";

        //    var response = await client.GetAsync(url);
        //    if (response.IsSuccessStatusCode)
        //    {
        //        var content = await response.Content.ReadAsStringAsync();
        //        mItems = JsonConvert.DeserializeObject<List<UserCartModel>>(content);
        //    }
        //    return mItems;
        //}

        public void BindData()
        {
            try
            {
        //        TableRow tableRow = new TableRow(this);
        //        TableRow.LayoutParams layoutParams = new TableRow.LayoutParams(
        //ViewGroup.LayoutParams.MatchParent,
        //ViewGroup.LayoutParams.MatchParent);

        //        foreach (var r in mItems)
        //        {

        //            productname = new TextView(this);
        //            productname.Text = r.ProductName;
        //            productname.LayoutParameters = layoutParams;

        //            price = new TextView(this);
        //            price.Text = Convert.ToString(r.Price);
        //            price.LayoutParameters = layoutParams;

        //            tableRow.AddView(price, 0);
        //            tableRow.AddView(price, 1);

        //            tablelayout.AddView(tableRow, 0);
        //        }
            }
            catch (Exception exx)
            {
                Console.WriteLine("Error During Bind Table Layout All Order Restaurent" + exx.Message);
            }
        }
    }
}