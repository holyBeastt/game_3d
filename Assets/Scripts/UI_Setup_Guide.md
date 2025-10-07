# Hướng dẫn Setup UI cho Game 3D

## Các file đã được tạo/cập nhật:

### 1. GameUI.cs (MỚI)
- Script quản lý tất cả UI panels (Game Over, Victory, Pause)
- Cung cấp các method để hiển thị thông báo thắng/thua
- Xử lý các nút restart và menu chính

### 2. GameManager.cs (ĐÃ CẬP NHẬT)
- Thêm kiểm tra điều kiện thắng khi đạt 50 coin
- Thêm method AddScore() để cập nhật điểm số an toàn
- Thêm biến victoryScore có thể điều chỉnh trong Inspector

### 3. Player.cs (ĐÃ CẬP NHẬT)
- Cập nhật method Die() để hiển thị Game Over UI thay vì reload scene ngay
- Thêm kiểm tra để tránh gọi Die() nhiều lần

### 4. Coin.cs (ĐÃ CẬP NHẬT)
- Sử dụng GameManager.AddScore() thay vì trực tiếp cập nhật score

## Cách setup trong Unity:

### Bước 1: Tạo UI Canvas
1. Tạo Canvas mới: Right-click trong Hierarchy → UI → Canvas
2. Đặt tên "GameUI"

### Bước 2: Tạo Game Over Panel
1. Right-click Canvas → UI → Panel
2. Đặt tên "GameOverPanel"
3. Thêm các UI elements:
   - Text (TMPro): "Game Over!" 
   - Text (TMPro): "Score: 0" (đặt tên "GameOverScoreText")
   - Button: "Restart" (đặt tên "RestartButton")
   - Button: "Main Menu" (đặt tên "MainMenuButton")

### Bước 3: Tạo Victory Panel
1. Right-click Canvas → UI → Panel
2. Đặt tên "VictoryPanel"
3. Thêm các UI elements:
   - Text (TMPro): "Victory!" 
   - Text (TMPro): "Final Score: 0" (đặt tên "VictoryScoreText")
   - Button: "Play Again" (đặt tên "VictoryRestartButton")
   - Button: "Main Menu" (đặt tên "VictoryMainMenuButton")

### Bước 4: Tạo Pause Panel (Tùy chọn)
1. Right-click Canvas → UI → Panel
2. Đặt tên "PausePanel"
3. Thêm các UI elements:
   - Text (TMPro): "Game Paused"
   - Button: "Resume" (đặt tên "ResumeButton")
   - Button: "Main Menu" (đặt tên "PauseMainMenuButton")

### Bước 5: Setup GameUI Script
1. Tạo Empty GameObject, đặt tên "GameUIManager"
2. Add component GameUI script
3. Kéo thả các UI elements vào các field tương ứng trong Inspector:
   - Game Over Panel → gameOverPanel
   - Victory Panel → victoryPanel
   - Pause Panel → pausePanel
   - Các Text components → scoreText fields
   - Các Button components → button fields

### Bước 6: Setup GameManager
1. Chọn GameObject có GameManager script
2. Trong Inspector, set Victory Score = 50 (hoặc số coin mong muốn)

## Tính năng:

### Thắng Game:
- Tự động hiển thị Victory panel khi đạt 50 coin
- Hiển thị điểm số cuối cùng
- Nút "Play Again" để chơi lại
- Nút "Main Menu" để về menu chính

### Thua Game:
- Hiển thị Game Over panel khi player rơi xuống dưới -5
- Hiển thị điểm số cuối cùng
- Nút "Restart" để chơi lại
- Nút "Main Menu" để về menu chính

### Pause Game:
- Nhấn Escape để pause/resume (cho testing)
- Có thể thêm nút pause trên UI mobile

## Lưu ý:
- Đảm bảo tên scene menu chính là "MainMenu" hoặc thay đổi trong GameUI.cs
- Tất cả panels sẽ được ẩn ban đầu và chỉ hiển thị khi cần
- Game sẽ tự động pause khi hiển thị Game Over hoặc Victory
- Có thể điều chỉnh victoryScore trong GameManager Inspector







