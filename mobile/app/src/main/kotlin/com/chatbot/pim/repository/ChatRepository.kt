package com.chatbot.pim.repository

import com.chatbot.pim.api.ApiClient
import com.chatbot.pim.api.ApiService
import com.chatbot.pim.api.models.ChatMessage
import com.chatbot.pim.api.models.ChatRequest
import com.chatbot.pim.api.models.ChatResponse
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.withContext

class ChatRepository {
    private val apiService = ApiClient.createService(ApiService::class.java)

    suspend fun sendMessage(messages: List<ChatMessage>): Result<ChatResponse> {
        return withContext(Dispatchers.IO) {
            try {
                val request = ChatRequest(messages)
                val response = apiService.sendMessage(request)

                if (response.isSuccessful && response.body() != null) {
                    val body = response.body()!!
                    if (body.success) {
                        Result.success(body)
                    } else {
                        Result.failure(Exception(body.message ?: "Erro ao enviar mensagem"))
                    }
                } else {
                    Result.failure(Exception("Erro ${response.code()}: ${response.message()}"))
                }
            } catch (e: Exception) {
                Result.failure(e)
            }
        }
    }
}
