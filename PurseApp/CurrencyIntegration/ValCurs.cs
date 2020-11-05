namespace PurseApp.CurrencyIntegration
{
	
	[System.SerializableAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
	[System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
	public partial class ValCurs
	{

		private ValCursValute[] valuteField;

		private string dateField;

		private string nameField;

		/// <remarks/>
		[System.Xml.Serialization.XmlElementAttribute("Valute")]
		public ValCursValute[] Valute
		{
			get { return this.valuteField; }
			set { this.valuteField = value; }
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string Date
		{
			get { return this.dateField; }
			set { this.dateField = value; }
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string name
		{
			get { return this.nameField; }
			set { this.nameField = value; }
		}
	}

	/// <remarks/>
	[System.SerializableAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
	public partial class ValCursValute
	{

		private ushort numCodeField;

		private string charCodeField;

		private ushort nominalField;

		private string nameField;

		private string valueField;

		private string idField;

		/// <remarks/>
		public ushort NumCode
		{
			get { return this.numCodeField; }
			set { this.numCodeField = value; }
		}

		/// <remarks/>
		public string CharCode
		{
			get { return this.charCodeField; }
			set { this.charCodeField = value; }
		}

		/// <remarks/>
		public ushort Nominal
		{
			get { return this.nominalField; }
			set { this.nominalField = value; }
		}

		/// <remarks/>
		public string Name
		{
			get { return this.nameField; }
			set { this.nameField = value; }
		}

		/// <remarks/>
		public string Value
		{
			get { return this.valueField; }
			set { this.valueField = value; }
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string ID
		{
			get { return this.idField; }
			set { this.idField = value; }
		}
	}
}