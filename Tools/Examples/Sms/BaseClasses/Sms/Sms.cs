
	// WARNING: Generated File! Do not modify by hand!
	// *************************************************************

using System;
using System.Collections.Generic;

public interface ISms :IPersistable <ISms>{
	public int Id { get; set; }
	public string Description { get; set; }
	public Category Category { get; set; }
	public User User { get; set; }
}

public abstract partial class Sms : IDescription, ISms {

	#region Fields

	private int id;
	private string description;
	private Category category;
	private User user;

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
	public Category Category
	{
		set{ category = value;}
		get{ return category;}
	}
	public User User
	{
		set{ user = value;}
		get{ return user;}
	}
	#endregion Properties

	#region IPersistable
				
	//TO DO: generate persistance class
	private static IPersistance<ISms> persistance = new SmsPersistance();
								
	public IPersistance<ISms> Persistance {
		get { return persistance; }
	}
				
	#endregion IPersistable
}