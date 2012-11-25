
	// WARNING: Generated File! Do not modify by hand!
	// *************************************************************

using System;
using System.Collections.Generic;

 interface IImageSms :IPersistable <IImageSms>{
	public string ImageUrl { get; set; }
}

  partial class ImageSms : Sms, IImageSms {

	#region Fields

	private string imageUrl;

	#endregion Fields

	#region Properties

	public string ImageUrl
	{
		set{ imageUrl = value;}
		get{ return imageUrl;}
	}
	#endregion Properties

	#region IPersistable
				
	//TO DO: generate persistance class
	private static IPersistance<IImageSms> persistance = new ImageSmsPersistance();
								
	public IPersistance<IImageSms> Persistance {
		get { return persistance; }
	}
				
	#endregion IPersistable
}