class PostMedia {
  int id;
  String mediaUrl;
  String postMediaType;
  String? thumbnailUrl;
  Duration? duration;
  String format;
  int size;
  bool? isUploaded;
  int? position;

  PostMedia({
    required this.id,
    required this.mediaUrl,
    required this.postMediaType,
    required this.thumbnailUrl,
    required this.duration,
    required this.format,
    required this.size,
    required this.isUploaded,
    required this.position,
  });

  static Duration? parseTimeSpan(String? timeSpan) {
    if (timeSpan == null) {
      return null;
    }
    final parts = timeSpan.split(':');
    if (parts.length != 3) {
      return null;
    }
    final hours = int.parse(parts[0]);
    final minutes = int.parse(parts[1]);
    final seconds = int.parse(parts[2]);
    return Duration(hours: hours, minutes: minutes, seconds: seconds);
  }

  factory PostMedia.fromJson(Map<String, dynamic> json) {
    return PostMedia(
      id: json['id'] as int,
      mediaUrl: json['mediaUrl'] as String,
      postMediaType: json['postMediaType'] as String,
      thumbnailUrl: json['thumbnailUrl'] as String?,
      duration: parseTimeSpan(json['duration']),
      format: json['format'] as String,
      size: json['size'] as int,
      isUploaded: json['isUploaded'] as bool?,
      position: json['position'] as int?,
    );
  }
}
