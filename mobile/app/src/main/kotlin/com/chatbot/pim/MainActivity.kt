package com.chatbot.pim

import android.os.Bundle
import androidx.activity.ComponentActivity
import androidx.activity.compose.setContent
import androidx.compose.runtime.collectAsState
import androidx.compose.runtime.getValue
import androidx.lifecycle.viewmodel.compose.viewModel
import androidx.navigation.NavHostController
import androidx.navigation.compose.NavHost
import androidx.navigation.compose.composable
import androidx.navigation.compose.rememberNavController
import com.chatbot.pim.ui.screens.ChatScreen
import com.chatbot.pim.ui.screens.LoginScreen
import com.chatbot.pim.ui.screens.RegisterScreen
import com.chatbot.pim.ui.theme.ChatBOTPIMTheme
import com.chatbot.pim.viewmodel.AuthViewModel
import com.chatbot.pim.viewmodel.ChatViewModel

class MainActivity : ComponentActivity() {
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContent {
            ChatBOTPIMTheme {
                AppNavigation()
            }
        }
    }
}

@Composable
fun AppNavigation() {
    val navController = rememberNavController()
    val authViewModel: AuthViewModel = viewModel()
    val chatViewModel: ChatViewModel = viewModel()
    val authUiState by authViewModel.uiState.collectAsState()

    // Se não está logado, mostrar Login
    if (!authUiState.isLoggedIn) {
        AuthNavigation(navController, authViewModel)
    } else {
        // Se está logado, mostrar Chat
        ChatScreen(
            chatViewModel = chatViewModel,
            user = authUiState.user,
            onLogout = {
                authViewModel.logout()
                chatViewModel.clearChat()
            }
        )
    }
}

@Composable
fun AuthNavigation(
    navController: NavHostController,
    authViewModel: AuthViewModel
) {
    NavHost(
        navController = navController,
        startDestination = "login"
    ) {
        composable("login") {
            LoginScreen(
                viewModel = authViewModel,
                onLoginSuccess = {
                    navController.navigate("chat") {
                        popUpTo("login") { inclusive = true }
                    }
                },
                onRegisterClick = {
                    navController.navigate("register")
                }
            )
        }

        composable("register") {
            RegisterScreen(
                viewModel = authViewModel,
                onRegisterSuccess = {
                    navController.navigate("chat") {
                        popUpTo("login") { inclusive = true }
                    }
                },
                onBackClick = {
                    navController.popBackStack()
                }
            )
        }
    }
}
