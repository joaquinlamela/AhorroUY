package com.ahorrouy.ui.user

import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.Button
import androidx.core.widget.doOnTextChanged
import androidx.fragment.app.Fragment
import androidx.fragment.app.activityViewModels
import androidx.lifecycle.Observer
import com.ahorrouy.R
import com.ahorrouy.api.model.signup.SignupRequest
import com.ahorrouy.ui.user.viewmodel.UserViewModel
import com.google.android.material.snackbar.Snackbar
import com.google.android.material.textfield.TextInputLayout

class SignupFragment : Fragment() {

    private val userViewModel: UserViewModel by activityViewModels()

    private lateinit var txtName: TextInputLayout
    private lateinit var txtUsername: TextInputLayout
    private lateinit var txtPassword: TextInputLayout
    private lateinit var txtPassword2: TextInputLayout
    private lateinit var btnSend: Button

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        return inflater.inflate(R.layout.fragment_signup, container, false)
    }

    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
        txtName = view.findViewById(R.id.txt_Name)
        txtUsername = view.findViewById(R.id.txt_Username)
        txtPassword = view.findViewById(R.id.txt_Password)
        txtPassword2 = view.findViewById(R.id.txt_Password2)
        btnSend = view.findViewById(R.id.btn_Send)

        txtName.editText?.doOnTextChanged { inputText, _, _, _ ->
            validateName(inputText.toString())
        }

        txtUsername.editText?.doOnTextChanged { inputText, _, _, _ ->
            validateUsername(inputText.toString())
        }

        txtPassword.editText?.doOnTextChanged { inputText, _, _, _ ->
            validatePass1(inputText.toString())
        }

        txtPassword2.editText?.doOnTextChanged { inputText, _, _, _ ->
            validatePass2(inputText.toString(), txtPassword.editText?.text.toString())
        }

        btnSend.setOnClickListener {
            if ((localValidation())) {
                sendForm()
                processResponse(view)
            }
        }

    }

    private fun localValidation(): Boolean {
        val name = txtName.editText?.text.toString()
        val email = txtUsername.editText?.text.toString()
        val pass1 = txtPassword.editText?.text.toString()
        val pass2 = txtPassword2.editText?.text.toString()

        return validateName(name) && validateUsername(email) &&
                validatePass1(pass1) && validatePass2(pass2, pass1)
    }

    private fun sendForm() {
        val newUser = SignupRequest(
            txtName.editText?.text.toString(),
            txtUsername.editText?.text.toString(),
            txtPassword.editText?.text.toString()
        )
        userViewModel.postUser(newUser)
    }

    private fun processResponse(view: View) {
        userViewModel.signUpResponse.observe(viewLifecycleOwner, Observer { response ->
            if (response.isSuccessful) {
                Snackbar.make(view, "Usuario creado exitosamente!", Snackbar.LENGTH_SHORT)
                    .setAction("OK") {}.show()
            } else {
                var errorCode = response.raw().code.toString()
                if (response.headers()["ErrorCode"] != null)
                    errorCode = response.headers()["ErrorCode"] as String
                handleResponseError(errorCode, view)
            }
        })
    }

    private fun handleResponseError(
        errorCode: String,
        view: View
    ) {
        var errorMsg: String = errorCode
        when (errorCode) {
            "500" -> errorMsg = "Error: No se pudo conectar al backend."
            "ERR_USER_NAME_EMPTY" -> errorMsg = "Debe ingresar su nombre."
            "ERR_USER_USERNAME_EMPTY" -> errorMsg = "Debe ingresar un nombre de usuario."
            "ERR_USER_PASSWORD_EMPTY" -> errorMsg = "Debe igresar una contraseña válida."
            "ERR_USER_USERNAME_ALREADY_EXISTS" -> errorMsg =
                "Ya existe un usuario con este nombre de usuario."
        }
        Snackbar.make(view, errorMsg, Snackbar.LENGTH_SHORT).setAction("OK") {}.show()
    }

    private fun validateName(name: String): Boolean {
        if (name.isEmpty()) {
            txtName.error = "Campo obligatorio"
            return false
        }
        txtName.error = null
        return true
    }

    private fun validateUsername(username: String): Boolean {
        if (username.isEmpty()) {
            txtUsername.error = "Campo obligatorio"
            return false
        }
        txtUsername.error = null
        return true
    }

    private fun validatePass1(pass1: String): Boolean {
        if (pass1.isEmpty()) {
            txtPassword.error = "Campo obligatorio"
            return false
        }
        txtPassword.error = null
        return true
    }

    private fun validatePass2(pass2: String, pass1: String): Boolean {
        if (pass2.isEmpty()) {
            txtPassword2.error = "Campo obligatorio"
            return false
        } else if (pass1 != pass2) {
            txtPassword2.error = "Contraseñas no coinciden"
            return false
        }
        txtPassword2.error = null
        return true
    }
}