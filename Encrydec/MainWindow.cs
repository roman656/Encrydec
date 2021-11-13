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
        private bool _hasInputTextFieldContent;
        private bool _hasKeyFieldContent;

        public MainWindow() : this(new Builder("MainWindow.glade")) {}

        private MainWindow(Builder builder) : base(builder.GetRawOwnedObject("MainWindow"))
        {
            builder.Autoconnect(this);

            DeleteEvent += WindowDeleteEvent;
            _startButton.Clicked += StartButtonClicked;
            _inputTextField.Buffer.Changed += InputTextFieldBufferChanged;
            _keyField.Buffer.Changed += KeyFieldBufferChanged;
        }

        private void WindowDeleteEvent(object sender, DeleteEventArgs a) => Application.Quit();

        private void StartButtonClicked(object sender, EventArgs a)
        {
            _outputTextField.Buffer.Text = _cryptoAlgorithmTypeField.Active switch
            {
                0 => "0",
                1 => "1",
                _ => "2"
            };
        }
        
        private void InputTextFieldBufferChanged(object sender, EventArgs a)
        {
            _hasInputTextFieldContent = _inputTextField.Buffer.CharCount > 0;
            UpdateStartButtonState();
        }
        
        private void KeyFieldBufferChanged(object sender, EventArgs a)
        {
            _hasKeyFieldContent = _keyField.Buffer.CharCount > 0;
            UpdateStartButtonState();
        }

        private void UpdateStartButtonState()
        {
            _startButton.Sensitive = _hasInputTextFieldContent && _hasKeyFieldContent;
        }
    }
}