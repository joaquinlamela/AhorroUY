package com.ahorrouy.ui.user

import android.app.Activity
import android.content.Context
import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.view.inputmethod.InputMethodManager
import android.widget.Button
import androidx.core.widget.doOnTextChanged
import androidx.fragment.app.Fragment
import androidx.fragment.app.activityViewModels
import androidx.navigation.findNavController
import com.ahorrouy.R
import com.ahorrouy.api.model.login.LoginRequest
import com.ahorrouy.ui.user.viewmodel.UserViewModel
import com.google.android.material.snackbar.Snackbar
import com.google.android.material.textfield.TextInputLayout

class LoginFragment : Fragment() {

    private val userViewModel: UserViewModel by activityViewModels()

    private lateinit var usernameTextInput: TextInputLayout
    private lateinit var passwordTextInput: TextInputLayout
    private lateinit var sendButton: Button

    private lateinit var credentials: LoginRequest

    private var snackBars = arrayListOf<Snackbar>()

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        return inflater.inflate(R.layout.fragment_login, container, false)
    }

    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
        usernameTextInput = view.findViewById(R.id.txt_Username)
        passwordTextInput = view.findViewById(R.id.txt_Password)
        sendButton = view.findViewById(R.id.btn_Send)

        usernameTextInput.editText?.doOnTextChanged { inputText, _, _, _ ->
            validateUsername(inputText.toString())
        }

        passwordTextInput.editText?.doOnTextChanged { inputText, _, _, _ ->
            validatePass(inputText.toString())
        }

        sendButton.setOnClickListener {
            if ((localValidation())) {
                sendForm()
                processResponse(view)
            }
        }
    }

    private fun sendForm() {
        credentials = LoginRequest(
            usernameTextInput.editText?.text.toString(),
            passwordTextInput.editText?.text.toString()
        )
        userViewModel.login(credentials)
    }

    private fun processResponse(view: View) {
        userViewModel.loginResponse.observe(viewLifecycleOwner, { response ->
            if (response != null) {
                if (response.isSuccessful) {
                    showSnackbar(view, "Iniciado sesión correctamente!")
                    handleResponseSuccessful(response.body()?.Token.toString(), view)
                } else {
                    var errorCode = response.raw().code.toString()
                    if (response.headers()["ErrorCode"] != null)
                        errorCode = response.headers()["ErrorCode"] as String
                    handleResponseError(errorCode, view)
                }
            }
        })
    }

    private fun handleResponseSuccessful(token: String, view: View) {
        var fileName = "currentUserToken"
        context?.openFileOutput(fileName, Context.MODE_PRIVATE).use {
            it?.write(token.toByteArray())
        }
        fileName = "currentUserName"
        context?.openFileOutput(fileName, Context.MODE_PRIVATE).use {
            it?.write(credentials.Username.toByteArray())
        }

        userViewModel.loginSuccess(token, credentials)

        val inputMethodManager =
            context?.getSystemService(Activity.INPUT_METHOD_SERVICE) as InputMethodManager
        inputMethodManager.hideSoftInputFromWindow(view.windowToken, 0)

        view.findNavController().navigate(R.id.nav_home)
    }

    private fun handleResponseError(errorCode: String, view: View) {
        var errorMsg: String = errorCode
        when (errorCode) {
            "500" -> errorMsg = "Error: No se pudo conectar al backend."
            "ERR_LOGIN_INCORRECT_CREDENTIALS" -> errorMsg =
                "Error: Usuario o contraseña incorrecto."
            "ERR_USER_USERNAME_EMPTY" -> errorMsg = "Debe ingresar un nombre de usuario."
            "ERR_USER_PASSWORD_EMPTY" -> errorMsg = "Debe igresar una contraseña válida."
        }
        showSnackbar(view, errorMsg)
    }

    private fun localValidation(): Boolean {
        val email = usernameTextInput.editText?.text.toString()
        val pass1 = passwordTextInput.editText?.text.toString()

        return validateUsername(email) && validatePass(pass1)
    }

    private fun validateUsername(username: String): Boolean {
        if (username.isEmpty()) {
            usernameTextInput.error = "Campo obligatorio"
            return false
        }
        usernameTextInput.error = null
        return true
    }

    private fun validatePass(pass1: String): Boolean {
        if (pass1.isEmpty()) {
            passwordTextInput.error = "Campo obligatorio"
            return false
        }
        passwordTextInput.error = null
        return true
    }

    private fun showSnackbar(view: View, msg: String) {
        if (snackBars.isNotEmpty()) {
            snackBars[0].dismiss()
            snackBars.clear()
        }
        val snackBar = Snackbar.make(view, msg, Snackbar.LENGTH_SHORT).setAction("OK") {}
        snackBars.add(snackBar)
        snackBar.show()
    }

}