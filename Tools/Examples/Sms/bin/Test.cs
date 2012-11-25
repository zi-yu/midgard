
using System;
using Loki.Generic;
using Sms.Core;
using Sms.DataAccessLayer;

public class TestClass {

public static void Main()
{
	try {
		//<property name="connection.connection_string">Server=192.168.1.5;Port=3306;Database=Midgard;Uid=mintaka;Pwd=spoon</property>
		//<property name="connection.connection_string">Server=localhost;Database=Midgard;User ID=mintaka;Password=spoon;Trusted_Connection=false</property>
		Log.Warn("$$$$$ Writing Schema File");
		NHibernateUtilities.DropSchema("SmsDrop.sql");
		NHibernateUtilities.CreateSchema("SmsCreate.sql");

		using (CategoryPersistance persistance = CategoryPersistance.GetSession()) {

			for (int i = 0; i < 8; ++i) {
				Log.Info("Creating Category {0}...", i);
				Category category = persistance.Create();
				category.Description = "Category " + i;
				persistance.Update(category);
			}
		}

		using (CategoryPersistance persistance = CategoryPersistance.GetSession()) {
			Log.Info("Categories");
			foreach (Category cat in persistance.Select()) {
				Log.Info("(id:{0}) {1}", cat.Id, cat.Description);
			}
		}

	} catch (Exception ex) {
		Log.Fatal(ex);
	}
}

};