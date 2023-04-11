# ListView Styling Sample
I was recently trying to figure out how to style a ListView. The project I am working on is a Monthly Budget helper.
That app will allow users to download statements from their credit card companies and assign categories to them. The
Categories will be from a fixed set of Categories that I have defined.

In my ListView I have defined the various GridViewColumns. For the columns that are not Category they simply display
the value in the Transaction. For the Category the idea was to have the column display the Category string. If the user
clicked on the Category I wanted to use a ComboBox and allow the user to choose one of the pre-defined categories. I
had a very hard time finding any real examples of this, but after playing around for awhile I was able to figure out
a way of doing it. I have since switched away from that idea for my app, but I thought I would preserve the styling so
I could do something similiar if ever needed.
