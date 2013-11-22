using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.Dialog;
using rotha.xamarin.element;

namespace DatePickerDemo
{
	public partial class DatePickerDemoViewController : DialogViewController
	{

		public DatePickerDemoViewController ()
			: base (UITableViewStyle.Grouped, new RootElement ("Demo"), true)
		{
			//NOTE: ENSURE THAT ROOT.UNEVENROWS IS SET TO TRUE 
			// OTHERWISE THE DatePickerElement.Height function is not called
			Root.UnevenRows = true;

			// Create section to hold date picker
			Section section = new Section ("Date Picker Test");

			// Create elements
			StringElement descriptionElement = new StringElement ("This demo shows how the date picker works within a section");
			DatePickerElement datePickerElement = new DatePickerElement ("Select date", section, DateTime.Now, UIDatePickerMode.DateAndTime);
			EntryElement entryElement = new EntryElement ("Example entry box", "test", "test");
			StringElement buttonElement = new StringElement ("Reset Date Picker", () => {
				// This is how you can set the date picker after it has been created
				datePickerElement.SelectedDate = DateTime.Now;
			});
			StringElement buttonFinalElement = new StringElement ("Show Selected Date", () => {
				// This is how you can access the selected date from the date picker
				entryElement.Value = datePickerElement.SelectedDate.ToString();
			});

			// Add to section
			section.AddAll (new Element[] { descriptionElement, datePickerElement, entryElement, buttonElement, buttonFinalElement });

			// Add section to root
			Root.Add (section);

		}

	}
}

