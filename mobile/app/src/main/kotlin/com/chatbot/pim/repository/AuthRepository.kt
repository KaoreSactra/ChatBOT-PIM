package com.chatbot.pim.repository

import com.chatbot.pim.api.ApiClient
import com.chatbot.pim.api.ApiService
import com.chatbot.pim.api.models.LoginRequest
import com.chatbot.pim.api.models.LoginResponse
import com.chatbot.pim.api.models.RegisterRequest
import com.chatbot.pim.api.models.UserResponse
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.withContext

class AuthRepository {
    private val apiService = ApiClient.createService(ApiService::class.java)

    suspend fun login(email: String, password: String): Result<Pair<LoginResponse, UserResponse>> {
        return withContext(Dispatchers.IO) {
            try {
                val request = LoginRequest(email, password)
                val response = apiService.login(request)

                if (response.isSuccessful && response.body() != null) {
                    val body = response.body()!!
                    if (body.success && body.user != null) {
                        Result.success(Pair(body, body.user))
                    } else {
                        Result.failure(Exception(body.message ?: "Erro ao fazer login"))
                    }
                } else {
                    Result.failure(Exception("Erro ${response.code()}: ${response.message()}"))
                }
            } catch (e: Exception) {
                Result.failure(e)
            }
        }
    }

    suspend fun register(email: String, password: String): Result<Pair<LoginResponse, UserResponse>> {
        return withContext(Dispatchers.IO) {
            try {
                val request = RegisterRequest(email, password, "user")
                val response = apiService.register(request)

                if (response.isSuccessful && response.body() != null) {
                    val body = response.body()!!
                    if (body.success && body.user != null) {
                        Result.success(Pair(body, body.user))
                    } else {
                        Result.failure(Exception(body.message ?: "Erro ao registrar"))
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
