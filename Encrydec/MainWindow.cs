using System;
using Encrydec.Ciphers;
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
            _cryptoAlgorithmTypeField.Changed += CryptoAlgorithmTypeFieldChanged;
        }

        private void WindowDeleteEvent(object sender, DeleteEventArgs a) => Application.Quit();

        private void StartButtonClicked(object sender, EventArgs a)
        {
            switch (_cryptoAlgorithmTypeField.Active)
            {
                case 0:
                {
                    var scytale = new Scytale(_inputTextField.Buffer.Text, Convert.ToInt32(_keyField.Buffer.Text));
                    
                    _outputTextField.Buffer.Text = _workModeField.Active == 0 ? scytale.EncryptedMessage
                            : scytale.DecryptedMessage;
                    break;
                }
                case 1:
                {
                    var polybiusSquare = new PolybiusSquare(_inputTextField.Buffer.Text,
                            _keyField.Buffer.Text);

                    _outputTextField.Buffer.Text = _workModeField.Active == 0 ? polybiusSquare.EncryptedMessage
                            : polybiusSquare.DecryptedMessage;
                    break;
                }
                default:
                {
                    var twoSquareCipher = new TwoSquareCipher(_inputTextField.Buffer.Text,
                            _keyField.Buffer.Text);

                    _outputTextField.Buffer.Text = _workModeField.Active == 0 ? twoSquareCipher.EncryptedMessage
                            : twoSquareCipher.DecryptedMessage;
                    break;
                }
            }
        }
        
        private void CryptoAlgorithmTypeFieldChanged(object sender, EventArgs a) => UpdateStartButtonState();

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
            _startButton.Sensitive = _hasInputTextFieldContent && _hasKeyFieldContent 
                    && (_cryptoAlgorithmTypeField.Active == 0
                    && Scytale.CheckKey(_keyField.Buffer.Text, _inputTextField.Buffer.CharCount)
                    || _cryptoAlgorithmTypeField.Active == 1
                    && PolybiusSquare.CheckKey(_keyField.Buffer.Text, _inputTextField.Buffer.Text)
                    || _cryptoAlgorithmTypeField.Active == 2
                    && TwoSquareCipher.CheckKey(_keyField.Buffer.Text, _inputTextField.Buffer.CharCount));
        }
    }
}