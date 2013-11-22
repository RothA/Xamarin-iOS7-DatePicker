using System;
using MonoTouch.Dialog;
using MonoTouch.UIKit;
using System.Drawing;
using MonoTouch.Foundation;

namespace rotha.xamarin.element
{
	/// <summary>
	/// Date picker element.
	/// </summary>
	public class DatePickerElement : StringElement
	{
		/// <summary>
		/// The format string for date times
		/// </summary>
		private static string DATE_TIME_FORMAT = "M/d/yy h:mm tt";
		private static string DATE_FORMAT = "M/d/yy";
		private static string TIME_FORMAT = "h:mm tt";
		/// <summary>
		/// The section.
		/// </summary>
		protected Section Section;
		/// <summary>
		/// The selected date.
		/// </summary>
		private DateTime selectedDate;
		/// <summary>
		/// The in edit mode.
		/// </summary>
		private bool inEditMode;
		/// <summary>
		/// The default color.
		/// </summary>
		private UIColor defaultColor;
		/// <summary>
		/// The root element.
		/// </summary>
		private RootElement RootElement;

		/// <summary>
		/// Gets or sets the selected date.
		/// </summary>
		/// <value>The selected date.</value>
		public DateTime SelectedDate {
			get {
				return this.selectedDate;
			}
			set {
				// When setting the date update the date picker
				this.selectedDate = value;

				if (this.DateElement != null) {
					this.DateElement.DatePicker.SetDate (this.selectedDate, true);
				}

				// update label
				this.updateLabel ();
			}
		}

		/// <summary>
		/// The date element.
		/// </summary>
		protected CustomDateElement DateElement;

		public DatePickerElement (String caption, Section section, DateTime defaultDate, UIDatePickerMode datePickerMode) : base (caption)
		{
			//init
			this.Section = section;
			this.inEditMode = false;

			// init date picker
			this.DateElement = new CustomDateElement ();
			this.DateElement.DatePicker.Mode = datePickerMode;

			// Set date
			this.selectedDate = defaultDate;

			// init value
			this.updateLabel ();

			//Setup datepicker change event
			this.DateElement.DatePicker.ValueChanged += (object sender, EventArgs e) => {

				// set selected date
				this.selectedDate = ((DateTime)this.DateElement.DatePicker.Date).ToLocalTime ();

				// refresh label
				this.updateLabel ();
			};

			// setup label click
			this.Tapped += () => {
				if ((this.Section.Count == (int)this.IndexPath.Item + 1) || (this.Section [(int)this.IndexPath.Item + 1].GetType () != typeof(CustomDateElement))){

					// Insert date picker element
					this.Section.Insert ((int)this.IndexPath.Item + 1, UITableViewRowAnimation.Automatic, new Element[]{ this.DateElement });

					//set edit mode
					this.inEditMode = true;

				} else {

					//remove
					this.Section.Remove ((int)this.IndexPath.Item + 1);

					// set edit mode
					this.inEditMode = false;
				}

				//force cell refresh
				this.RootElement = this.GetImmediateRootElement ();
				this.RootElement.Reload (this, UITableViewRowAnimation.None);
			};
		}

		public override UITableViewCell GetCell (UITableView tv)
		{
			UITableViewCell cell = base.GetCell (tv);

			if (this.inEditMode) {
				this.defaultColor = cell.DetailTextLabel.TextColor;
				cell.DetailTextLabel.TextColor = UIColor.Red;
			} else {
				if (this.defaultColor != null) {
					cell.DetailTextLabel.TextColor = this.defaultColor;
				}
			}
			return cell;
		}
		// Updates the label
		protected void updateLabel ()
		{
			// updates the label with the current timestamp
			UIApplication.SharedApplication.InvokeOnMainThread (() => {

				//set value
				switch (this.DateElement.DatePicker.Mode) {
				case UIDatePickerMode.Date:
					this.Value = this.SelectedDate.ToString (DATE_FORMAT);
					break;
				case UIDatePickerMode.Time:
					this.Value = this.SelectedDate.ToString (TIME_FORMAT);
					break;
				default:
					this.Value = this.SelectedDate.ToString (DATE_TIME_FORMAT);
					break;
				}

				if (this.RootElement != null) {
				this.RootElement.Reload (this, UITableViewRowAnimation.None);
				}
			});
		}
	}

	/// <summary>
	/// Custom date element.
	/// </summary>
	public class CustomDateElement : OwnerDrawnElement
	{
		/// <summary>
		/// Gets or sets the date picker.
		/// </summary>
		/// <value>The date picker.</value>
		public UIDatePicker DatePicker {
			get;
			set;
		}

		public CustomDateElement () : base (UITableViewCellStyle.Default, "datePickerElement")
		{
			// initialize date picker
			this.DatePicker = new UIDatePicker (Rectangle.Empty);
		}

		public override void Draw (RectangleF bounds, MonoTouch.CoreGraphics.CGContext context, UIView view)
		{
			// Remove subviews from superview if already added
			if (view.Subviews.Length > 0) {

				// Remove view from subview
				view.Subviews [0].RemoveFromSuperview ();
			}

			// Set the frame of the UIDatePicker to the incoming bounds
			this.DatePicker.Frame = bounds;

			// Set backgroundcolor to white
			UIColor.White.SetFill ();

			// Fill background
			context.FillRect (bounds);

			// set font to black
			UIColor.Black.SetColor ();

			// add date picker to view
			view.AddSubview (this.DatePicker);
		}

		public override float Height (RectangleF bounds)
		{
			// return height of date picker
			return 200f;
		}
	}
}
