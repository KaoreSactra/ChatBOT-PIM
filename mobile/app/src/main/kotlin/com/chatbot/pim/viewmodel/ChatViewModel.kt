package com.chatbot.pim.viewmodel

import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.chatbot.pim.api.models.ChatMessage
import com.chatbot.pim.repository.ChatRepository
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.StateFlow
import kotlinx.coroutines.launch

data class Message(
    val id: String = System.currentTimeMillis().toString(),
    val role: String,
    val content: String
)

data class ChatUiState(
    val messages: List<Message> = emptyList(),
    val isLoading: Boolean = false,
    val error: String? = null,
    val inputText: String = ""
)

class ChatViewModel : ViewModel() {
    private val chatRepository = ChatRepository()

    private val _uiState = MutableStateFlow(ChatUiState())
    val uiState: StateFlow<ChatUiState> = _uiState

    fun setInputText(text: String) {
        _uiState.value = _uiState.value.copy(inputText = text)
    }

    fun clearError() {
        _uiState.value = _uiState.value.copy(error = null)
    }

    fun sendMessage() {
        val text = _uiState.value.inputText.trim()
        if (text.isEmpty()) return

        // Adicionar mensagem do usuário
        val userMessage = Message(role = "user", content = text)
        val updatedMessages = _uiState.value.messages + userMessage

        _uiState.value = _uiState.value.copy(
            messages = updatedMessages,
            inputText = "",
            isLoading = true,
            error = null
        )

        // Enviar para API
        viewModelScope.launch {
            // Converter para ChatMessage para o formato da API
            val apiMessages = updatedMessages.map {
                ChatMessage(role = it.role, content = it.content)
            }

            val result = chatRepository.sendMessage(apiMessages)
            result.onSuccess { response ->
                // Adicionar resposta do bot
                val assistantMessage = Message(role = "assistant", content = response.response)
                val finalMessages = updatedMessages + assistantMessage

                _uiState.value = _uiState.value.copy(
                    messages = finalMessages,
                    isLoading = false,
                    error = null
                )
            }.onFailure { exception ->
                // Remover mensagem do usuário se houve erro
                _uiState.value = _uiState.value.copy(
                    messages = _uiState.value.messages.dropLast(1),
                    isLoading = false,
                    error = exception.message ?: "Erro ao enviar mensagem",
                    inputText = text // Restaurar texto
                )
            }
        }
    }

    fun clearChat() {
        _uiState.value = ChatUiState()
    }
}
