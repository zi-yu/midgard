
	// WARNING: Generated File! Do not modify by hand!
	// *************************************************************

using System;
using System.Collections.Generic;

public interface ICategory :IPersistable <ICategory>{
	public int Id { get; set; }
	public string Description { get; set; }
	public Sms Messages { get; set; }
}

public  partial class Category : IDescription, ICategory {

	#region Fields

	private int id;
	private string description;
	private Sms messages;

	#endregion Fields

	#region Properties

	public int Id
	{
		set{ id = value;}
		get{ return id;}
	}
	public string Description
	{
		set{ description = value;}
		get{ return description;}
	}
	public Sms Messages
	{
		set{ messages = value;}
		get{ return messages;}
	}
	#endregion Properties

	#region IPersistable
				
	//TO DO: generate persistance class
	private static IPersistance<ICategory> persistance = new CategoryPersistance();

	public static IPersistance<ICategory> Persistance {
		get { return persistance; }
	}
				
	#endregion IPersistable
}