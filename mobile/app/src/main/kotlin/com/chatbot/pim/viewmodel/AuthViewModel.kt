package com.chatbot.pim.viewmodel

import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.chatbot.pim.api.models.UserResponse
import com.chatbot.pim.repository.AuthRepository
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.StateFlow
import kotlinx.coroutines.launch

data class AuthUiState(
    val isLoading: Boolean = false,
    val isLoggedIn: Boolean = false,
    val user: UserResponse? = null,
    val error: String? = null,
    val email: String = "",
    val password: String = "",
    val confirmPassword: String = ""
)

class AuthViewModel : ViewModel() {
    private val authRepository = AuthRepository()

    private val _uiState = MutableStateFlow(AuthUiState())
    val uiState: StateFlow<AuthUiState> = _uiState

    fun setEmail(email: String) {
        _uiState.value = _uiState.value.copy(email = email)
    }

    fun setPassword(password: String) {
        _uiState.value = _uiState.value.copy(password = password)
    }

    fun setConfirmPassword(password: String) {
        _uiState.value = _uiState.value.copy(confirmPassword = password)
    }

    fun clearError() {
        _uiState.value = _uiState.value.copy(error = null)
    }

    fun login() {
        val email = _uiState.value.email.trim()
        val password = _uiState.value.password

        if (email.isEmpty() || password.isEmpty()) {
            _uiState.value = _uiState.value.copy(error = "Email e senha são obrigatórios")
            return
        }

        _uiState.value = _uiState.value.copy(isLoading = true, error = null)

        viewModelScope.launch {
            val result = authRepository.login(email, password)
            result.onSuccess { (response, user) ->
                _uiState.value = _uiState.value.copy(
                    isLoading = false,
                    isLoggedIn = true,
                    user = user,
                    error = null,
                    email = "",
                    password = ""
                )
            }.onFailure { exception ->
                _uiState.value = _uiState.value.copy(
                    isLoading = false,
                    error = exception.message ?: "Erro ao fazer login"
                )
            }
        }
    }

    fun register() {
        val email = _uiState.value.email.trim()
        val password = _uiState.value.password
        val confirmPassword = _uiState.value.confirmPassword

        when {
            email.isEmpty() -> {
                _uiState.value = _uiState.value.copy(error = "Email é obrigatório")
            }
            password.isEmpty() -> {
                _uiState.value = _uiState.value.copy(error = "Senha é obrigatória")
            }
            password != confirmPassword -> {
                _uiState.value = _uiState.value.copy(error = "As senhas não coincidem")
            }
            password.length < 6 -> {
                _uiState.value = _uiState.value.copy(error = "Senha deve ter no mínimo 6 caracteres")
            }
            else -> {
                _uiState.value = _uiState.value.copy(isLoading = true, error = null)
                viewModelScope.launch {
                    val result = authRepository.register(email, password)
                    result.onSuccess { (response, user) ->
                        _uiState.value = _uiState.value.copy(
                            isLoading = false,
                            isLoggedIn = true,
                            user = user,
                            error = null,
                            email = "",
                            password = "",
                            confirmPassword = ""
                        )
                    }.onFailure { exception ->
                        _uiState.value = _uiState.value.copy(
                            isLoading = false,
                            error = exception.message ?: "Erro ao registrar"
                        )
                    }
                }
            }
        }
    }

    fun logout() {
        _uiState.value = AuthUiState()
    }
}
