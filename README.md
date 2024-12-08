# Multiplayer
## Hướng dẫn chạy game
1. Mở scene Gameplay trong folder scene
2. Sử dụng ParrelSync để clone ra nhiều thêm project clone Hoặc File?Build Setting và build với platform Window
3. Ấn Play trên editor nếu sử dụng ParrelSync, hoặc chạy nhiều file .exe được build ra
4. Click Host trên 1 app, các app còn lại click vào Client
5. Host Click Start, để bắt đầu game
## Công cụ sử dụng trong dự án
NetCode và package Multiplayer Samples Utilities
### Giải thích các quyết định đã sử dụng
1. Chưa thật sự hiểu rõ cách tạo room của NetCode nên đã sử dụng luôn sample có sẵn của NetCode => sẽ nghiên cứu và tìm hiểu thêm.
2. Logic Gameplay: cũng giống như photon-fusion hay mirror, việc triển khai logic cho game play không có gì quá khó khăn và phức tạp, hoàn toàn làm chủ được logic làm việc giữa Host-Client
3. Giới hạn 4 người chơi:
Do chỉ có nhiều kinh nghiệm với photon-fusion phần này ở bên fusion thì được set sẵn ngay khi host tạo room => dễ dàng để thiết lập.
Còn với NetCode đã tìm hiểu thử và thấy phần xử lý khá phức tạp nên chưa áp dụng vào trong prj này như các tài liệu tìm hiểu được.
Hiện tại chức năng này hoạt động như sau: Khi có connect mới Host sẽ kiểm tra số lượng Client connect vào ở function OnClientConnectedCallback, nếu đã đủ số lượng thì sẽ ngắt kết nối với Client đó. 
4. Chưa có triển khai (Lag Handling)
Do thời gian ngắn nên chưa kịp thời để hiểu tool mô phỏng được độ trễ như yêu cầu đề ra: Về cơ bản để xử lý vấn đề này thì có giải pháp sử dụng Interpolate/Extrapolate cho các đối tượng sử dụng di chuyển bằgn vật lý hoặc Prediction cho các đối tượng di chuyển bằng cả vật lý và transform