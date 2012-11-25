
	// WARNING: Generated File! Do not modify by hand!
	// *************************************************************

using System;
using System.Collections.Generic;

 interface IUser :IPersistable <IUser>{
	public int Id { get; set; }
	public Sms Messages { get; set; }
}

  partial class User : IUser {

	#region Fields

	private int id;
	private Sms messages;

	#endregion Fields

	#region Properties

	public int Id
	{
		set{ id = value;}
		get{ return id;}
	}
	public Sms Messages
	{
		set{ messages = value;}
		get{ return messages;}
	}
	#endregion Properties

	#region IPersistable
				
	//TO DO: generate persistance class
	private static IPersistance<IUser> persistance = new UserPersistance();
								
	public IPersistance<IUser> Persistance {
		get { return persistance; }
	}
				
	#endregion IPersistable
}