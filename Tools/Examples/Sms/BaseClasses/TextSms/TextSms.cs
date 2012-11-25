
	// WARNING: Generated File! Do not modify by hand!
	// *************************************************************

using System;
using System.Collections.Generic;

 interface ITextSms :IPersistable <ITextSms>{
	public string Text { get; set; }
}

  partial class TextSms : Sms, ITextSms {

	#region Fields

	private string text;

	#endregion Fields

	#region Properties

	public string Text
	{
		set{ text = value;}
		get{ return text;}
	}
	#endregion Properties

	#region IPersistable
				
	//TO DO: generate persistance class
	private static IPersistance<ITextSms> persistance = new TextSmsPersistance();
								
	public IPersistance<ITextSms> Persistance {
		get { return persistance; }
	}
				
	#endregion IPersistable
}