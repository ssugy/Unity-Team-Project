[![팀프로젝트](http://img.youtube.com/vi/AjfSB3DkwkE/0.jpg)](https://youtu.be/AjfSB3DkwkE) (클릭하시면 플레이 영상을 볼 수 있습니다.)
<br>
<br>
# Atens GameAcademy Team B Project
아텐츠 게임 아카데미 B조 팀 프로젝트<br>
[밀라노트(Milanote) - 전체 기획 및 진행사항 정리](https://app.milanote.com/1O8X0x10HN5E4X?p=wspKgn1E0uS)
<br><br>
## 구성원
* 권용훈(팀장) [이력서(Resume)](https://shimmering-fibre-cb8.notion.site/Dev_YH-resume-0b9fce7125684f229fa313cc444d3f6f)
  - 프로젝트 전체 관리, 조정
* 이주혁
  - 멀티플레이, 전체 코드 관리, 월드 구현, 던전 구현 등
* 차주영
  - 핵심 전투 구현(보스전투), 부위파괴 구현, 데이터 처리, 로비 구현 등
* 홍예나
  - 캐릭터 커스터마이징, 아이템 옵션 및 기능 추가, 상점 구현, 보스전투(초기) 등
* 천승환
  - 몬스터 일부기능, 버프기능 구현
<br><br>
## 게임 소개
모바일 멀티플레이용 3D RPG게임입니다.<br>
제작기간은 약2달(기능구현) + 1달(CodeRefactoring)입니다.<br>
1차 발표 후 포지션 변경을 통해서 구성원들이 타인의 코드를 분석하고 리팩토링 하는 훈련을 하였습니다.
<br><br>
## 프로젝트에 적용된 기술 정리
1. Multiplay<br>
   1) Photon을 이용한 구현
   2) 월드에 진입한 순간부터 멀티플레이 가능. 던전입장 시 sole모드와 multiplay모드 2가지로 나누어집니다.
2. BlendShape
   1) 케릭터 생성 시 커스터마이징 기능에 추가됨
3. Multilingual support
   1) Google spreadSheet 연동을 통한 다국어 번역 지원
4. Boss Combat
   1) Cut Scene : Unity Cinemachine을 이용한 컷신 구현 및 퀘스트 카메라 구현
   2) Part Destruction
   3) Animation Event Function을 통한 정확한 타이밍의 로직 처리 구현
5. 인벤토리, 상점, 강화, 퀘스트, 전투버프, UI편의성 등
