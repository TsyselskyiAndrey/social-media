class User {
  String firstName;
  String lastName;
  String email;
  String userName;
  String? biography;
  String? profileImagePath;
  DateTime? birthDate;
  int followed;
  int followers;
  int postsAmount;

  User({
    required this.firstName,
    required this.lastName,
    required this.email,
    required this.userName,
    required this.followed,
    required this.followers,
    required this.postsAmount,
    this.biography,
    this.profileImagePath,
    this.birthDate,
  });

  factory User.fromJson(Map<String, dynamic> json) {
    return User(
      firstName: json['firstName'] as String,
      lastName: json['lastName'] as String,
      email: json['email'] as String,
      userName: json['userName'] as String,
      biography: json['biography'] as String?,
      profileImagePath: json['profileImagePath'] as String?,
      birthDate: json['birthDate'] != null
          ? DateTime.parse(json['birthDate'] as String)
          : null,
      followed: json['followed'] as int,
      followers: json['followers'] as int,
      postsAmount: json['postsAmount'] as int,
    );
  }
}
