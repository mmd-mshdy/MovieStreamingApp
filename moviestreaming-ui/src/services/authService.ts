// Define the backend's base URL (Update this to match your local C# API port!)
const API_URL = "https://localhost:7049/Api/User"; 

export interface LoginResponse {
  id: string;
  name: string;
  email: string;
  token: string;
}

export const authService = {
  // 1. Sends the credentials DTO to your C# UserController.cs login endpoint
  // 1. Fixed the syntax: separated email and password with a comma!
  async login(email: string, password: string): Promise<LoginResponse> {
    const response = await fetch(`${API_URL}/login`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify({ email, password }), // This will work perfectly now!
    });

    if (!response.ok) {
      const errorText = await response.text();
      throw new Error(errorText || "Invalid credentials");
    }

    const data: LoginResponse = await response.json();
    
    // 2. Securely cache the cryptographic JWT token string in the browser's local storage
    localStorage.setItem("token", data.token);
    localStorage.setItem("user", JSON.stringify({ id: data.id, name: data.name, email: data.email }));

    return data;
  },

  // 3. Clear token data on logout
  logout() {
    localStorage.removeItem("token");
    localStorage.removeItem("user");
  },

  // 4. Helper helper to automatically attach the Bearer token to secure requests
  getAuthHeader(): Record<string, string> {
    const token = localStorage.getItem("token");
    return token ? { Authorization: `Bearer ${token}` } : {};
  }
};