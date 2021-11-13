using System;
using Gtk;
using UI = Gtk.Builder.ObjectAttribute;

namespace Encrydec
{
    public class MainWindow : Window
    {
        [UI] private Label _inputTextFieldLabel;
        [UI] private Label _outputTextFieldLabel;
        [UI] private Label _keyFieldLabel;
        [UI] private Label _workModeFieldLabel;
        [UI] private Label _cryptoAlgorithmTypeFieldLabel;
        [UI] private Button _startButton;
        [UI] private TextView _inputTextField;
        [UI] private TextView _outputTextField;
        [UI] private TextView _keyField;
        [UI] private ComboBox _workModeField;
        [UI] private ComboBox _cryptoAlgorithmTypeField;

        public MainWindow() : this(new Builder("MainWindow.glade")) {}

        private MainWindow(Builder builder) : base(builder.GetRawOwnedObject("MainWindow"))
        {
            builder.Autoconnect(this);

            DeleteEvent += Window_DeleteEvent;
            _startButton.Clicked += StartButtonClicked;
        }

        private void Window_DeleteEvent(object sender, DeleteEventArgs a)
        {
            Application.Quit();
        }

        private void StartButtonClicked(object sender, EventArgs a)
        {
            _outputTextField.Buffer.Text += "хоп";
        }
    }
}