namespace Encrydec;

using System;
using Ciphers;
using Gtk;
using UI = Gtk.Builder.ObjectAttribute;

public class MainWindow : Window
{
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

        DeleteEvent += WindowDeleteEvent;
        _startButton.Clicked += StartButtonClicked;
        _inputTextField.Buffer.Changed += InputTextFieldBufferChanged;
        _keyField.Buffer.Changed += KeyFieldBufferChanged;
        _cryptoAlgorithmTypeField.Changed += CryptoAlgorithmTypeFieldChanged;
    }

    private void WindowDeleteEvent(object sender, DeleteEventArgs a) => Application.Quit();
    private void CryptoAlgorithmTypeFieldChanged(object sender, EventArgs a) => UpdateStartButtonState();
    private void InputTextFieldBufferChanged(object sender, EventArgs a) => UpdateStartButtonState();
    private void KeyFieldBufferChanged(object sender, EventArgs a) => UpdateStartButtonState();

    private void StartButtonClicked(object sender, EventArgs a)
    {
        switch ((CipherType)_cryptoAlgorithmTypeField.Active)
        {
            case CipherType.Scytale:
            {
                var scytale = new Scytale(_inputTextField.Buffer.Text, Convert.ToInt32(_keyField.Buffer.Text));
                    
                _outputTextField.Buffer.Text = _workModeField.Active == 0 ? scytale.EncryptedMessage
                        : scytale.DecryptedMessage;
                break;
            }
            case CipherType.PolybiusSquare:
            {
                var polybiusSquare = new PolybiusSquare(_inputTextField.Buffer.Text, _keyField.Buffer.Text);

                _outputTextField.Buffer.Text = _workModeField.Active == 0 ? polybiusSquare.EncryptedMessage
                        : polybiusSquare.DecryptedMessage;
                break;
            }
            case CipherType.TwoSquareCipher:
            {
                var twoSquareCipher = new TwoSquareCipher(_inputTextField.Buffer.Text, _keyField.Buffer.Text);

                _outputTextField.Buffer.Text = _workModeField.Active == 0 ? twoSquareCipher.EncryptedMessage
                        : twoSquareCipher.DecryptedMessage;
                break;
            }
            case CipherType.Gronsfeld:
            {
                var gronsfeld = new Gronsfeld(_inputTextField.Buffer.Text, _keyField.Buffer.Text);
                    
                _outputTextField.Buffer.Text = _workModeField.Active == 0 ? gronsfeld.EncryptedMessage
                        : gronsfeld.DecryptedMessage;
                break;
            }
            default:
            {
                var singlePermutation = new SinglePermutation(_inputTextField.Buffer.Text, _keyField.Buffer.Text);

                _outputTextField.Buffer.Text = _workModeField.Active == 0 ? singlePermutation.EncryptedMessage
                        : singlePermutation.DecryptedMessage;
                break;
            }
        }
    }

    private void UpdateStartButtonState()
    {
        _startButton.Sensitive = CiphersParametersValidator.CheckMessageAndKey(_keyField.Buffer.Text,
                _inputTextField.Buffer.Text, (CipherType)_cryptoAlgorithmTypeField.Active);

    }
}