-- MySQL dump 10.13  Distrib 8.0.36, for Win64 (x86_64)
--
-- Host: localhost    Database: space_rythm_db
-- ------------------------------------------------------
-- Server version	8.0.36

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `__efmigrationshistory`
--

DROP TABLE IF EXISTS `__efmigrationshistory`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `__efmigrationshistory` (
  `MigrationId` varchar(150) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `ProductVersion` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`MigrationId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `__efmigrationshistory`
--

LOCK TABLES `__efmigrationshistory` WRITE;
/*!40000 ALTER TABLE `__efmigrationshistory` DISABLE KEYS */;
INSERT INTO `__efmigrationshistory` VALUES ('20240924191255_InitialCreate','8.0.8'),('20240926203156_InitialCreate1','8.0.8'),('20240927182052_AddTrackArtistTable','8.0.8'),('20240927191356_AddPlaylistAndPlaylistTracks','8.0.8'),('20240927194200_AddLikesFollowersCommentsTables','8.0.8'),('20240927195215_AddLikesFollowersTables','8.0.8'),('20240928174037_AddSubscriptionsAdminLogsTable','8.0.8');
/*!40000 ALTER TABLE `__efmigrationshistory` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `adminlogs`
--

DROP TABLE IF EXISTS `adminlogs`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `adminlogs` (
  `LogId` int NOT NULL AUTO_INCREMENT,
  `AdminId` int NOT NULL,
  `ActionType` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `TargetId` int DEFAULT NULL,
  `Timestamp` datetime(6) NOT NULL,
  PRIMARY KEY (`LogId`),
  KEY `IX_AdminLogs_AdminId` (`AdminId`),
  CONSTRAINT `FK_AdminLogs_Users_AdminId` FOREIGN KEY (`AdminId`) REFERENCES `users` (`user_id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `adminlogs`
--

LOCK TABLES `adminlogs` WRITE;
/*!40000 ALTER TABLE `adminlogs` DISABLE KEYS */;
/*!40000 ALTER TABLE `adminlogs` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `artists`
--

DROP TABLE IF EXISTS `artists`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `artists` (
  `ArtistId` int NOT NULL AUTO_INCREMENT,
  `Name` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Bio` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`ArtistId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `artists`
--

LOCK TABLES `artists` WRITE;
/*!40000 ALTER TABLE `artists` DISABLE KEYS */;
/*!40000 ALTER TABLE `artists` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `artists_liked`
--

DROP TABLE IF EXISTS `artists_liked`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `artists_liked` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `UserId` int NOT NULL,
  `ArtistId` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_artists_liked_UserId` (`UserId`),
  CONSTRAINT `FK_artists_liked_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `users` (`user_id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `artists_liked`
--

LOCK TABLES `artists_liked` WRITE;
/*!40000 ALTER TABLE `artists_liked` DISABLE KEYS */;
INSERT INTO `artists_liked` VALUES (1,1,'artist1'),(2,1,'artist2'),(3,2,'artist3'),(4,3,'artist1'),(5,3,'artist4');
/*!40000 ALTER TABLE `artists_liked` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `categories_liked`
--

DROP TABLE IF EXISTS `categories_liked`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `categories_liked` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `UserId` int NOT NULL,
  `CategoryId` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_categories_liked_UserId` (`UserId`),
  CONSTRAINT `FK_categories_liked_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `users` (`user_id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `categories_liked`
--

LOCK TABLES `categories_liked` WRITE;
/*!40000 ALTER TABLE `categories_liked` DISABLE KEYS */;
INSERT INTO `categories_liked` VALUES (1,1,'category1'),(2,1,'category2'),(3,2,'category3'),(4,3,'category1'),(5,3,'category4');
/*!40000 ALTER TABLE `categories_liked` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `comments`
--

DROP TABLE IF EXISTS `comments`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `comments` (
  `CommentId` int NOT NULL AUTO_INCREMENT,
  `TrackId` int NOT NULL,
  `UserId` int NOT NULL,
  `Content` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `PostedDate` datetime(6) NOT NULL,
  PRIMARY KEY (`CommentId`),
  KEY `IX_Comments_TrackId` (`TrackId`),
  KEY `IX_Comments_UserId` (`UserId`),
  CONSTRAINT `FK_Comments_Tracks_TrackId` FOREIGN KEY (`TrackId`) REFERENCES `tracks` (`TrackId`) ON DELETE CASCADE,
  CONSTRAINT `FK_Comments_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `users` (`user_id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `comments`
--

LOCK TABLES `comments` WRITE;
/*!40000 ALTER TABLE `comments` DISABLE KEYS */;
/*!40000 ALTER TABLE `comments` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `followers`
--

DROP TABLE IF EXISTS `followers`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `followers` (
  `UserId` int NOT NULL,
  `FollowedUserId` int NOT NULL,
  `FollowDate` datetime(6) NOT NULL,
  PRIMARY KEY (`UserId`,`FollowedUserId`),
  KEY `IX_Followers_FollowedUserId` (`FollowedUserId`),
  CONSTRAINT `FK_Followers_Users_FollowedUserId` FOREIGN KEY (`FollowedUserId`) REFERENCES `users` (`user_id`) ON DELETE CASCADE,
  CONSTRAINT `FK_Followers_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `users` (`user_id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `followers`
--

LOCK TABLES `followers` WRITE;
/*!40000 ALTER TABLE `followers` DISABLE KEYS */;
/*!40000 ALTER TABLE `followers` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `likes`
--

DROP TABLE IF EXISTS `likes`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `likes` (
  `UserId` int NOT NULL,
  `TrackId` int NOT NULL,
  `LikedDate` datetime(6) NOT NULL,
  PRIMARY KEY (`UserId`,`TrackId`),
  KEY `IX_Likes_TrackId` (`TrackId`),
  CONSTRAINT `FK_Likes_Tracks_TrackId` FOREIGN KEY (`TrackId`) REFERENCES `tracks` (`TrackId`) ON DELETE CASCADE,
  CONSTRAINT `FK_Likes_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `users` (`user_id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `likes`
--

LOCK TABLES `likes` WRITE;
/*!40000 ALTER TABLE `likes` DISABLE KEYS */;
/*!40000 ALTER TABLE `likes` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `playlists`
--

DROP TABLE IF EXISTS `playlists`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `playlists` (
  `PlaylistId` int NOT NULL AUTO_INCREMENT,
  `UserId` int NOT NULL,
  `Title` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Description` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `CreatedDate` datetime(6) NOT NULL,
  `IsPublic` tinyint(1) NOT NULL,
  PRIMARY KEY (`PlaylistId`),
  KEY `IX_Playlists_UserId` (`UserId`),
  CONSTRAINT `FK_Playlists_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `users` (`user_id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `playlists`
--

LOCK TABLES `playlists` WRITE;
/*!40000 ALTER TABLE `playlists` DISABLE KEYS */;
/*!40000 ALTER TABLE `playlists` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `playlisttracks`
--

DROP TABLE IF EXISTS `playlisttracks`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `playlisttracks` (
  `PlaylistId` int NOT NULL,
  `TrackId` int NOT NULL,
  `AddedDate` datetime(6) NOT NULL,
  PRIMARY KEY (`PlaylistId`,`TrackId`),
  KEY `IX_PlaylistTracks_TrackId` (`TrackId`),
  CONSTRAINT `FK_PlaylistTracks_Playlists_PlaylistId` FOREIGN KEY (`PlaylistId`) REFERENCES `playlists` (`PlaylistId`) ON DELETE CASCADE,
  CONSTRAINT `FK_PlaylistTracks_Tracks_TrackId` FOREIGN KEY (`TrackId`) REFERENCES `tracks` (`TrackId`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `playlisttracks`
--

LOCK TABLES `playlisttracks` WRITE;
/*!40000 ALTER TABLE `playlisttracks` DISABLE KEYS */;
/*!40000 ALTER TABLE `playlisttracks` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `songs_liked`
--

DROP TABLE IF EXISTS `songs_liked`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `songs_liked` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `UserId` int NOT NULL,
  `SongId` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_songs_liked_UserId` (`UserId`),
  CONSTRAINT `FK_songs_liked_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `users` (`user_id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `songs_liked`
--

LOCK TABLES `songs_liked` WRITE;
/*!40000 ALTER TABLE `songs_liked` DISABLE KEYS */;
INSERT INTO `songs_liked` VALUES (1,1,'song1'),(2,1,'song2'),(3,2,'song3'),(4,3,'song4'),(5,3,'song1');
/*!40000 ALTER TABLE `songs_liked` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `subscriptions`
--

DROP TABLE IF EXISTS `subscriptions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `subscriptions` (
  `SubscriptionId` int NOT NULL AUTO_INCREMENT,
  `UserId` int NOT NULL,
  `Type` int NOT NULL,
  `SubscriptionStartDate` datetime(6) NOT NULL,
  `SubscriptionEndDate` datetime(6) DEFAULT NULL,
  PRIMARY KEY (`SubscriptionId`),
  KEY `IX_Subscriptions_UserId` (`UserId`),
  CONSTRAINT `FK_Subscriptions_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `users` (`user_id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `subscriptions`
--

LOCK TABLES `subscriptions` WRITE;
/*!40000 ALTER TABLE `subscriptions` DISABLE KEYS */;
/*!40000 ALTER TABLE `subscriptions` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `trackmetadatas`
--

DROP TABLE IF EXISTS `trackmetadatas`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `trackmetadatas` (
  `MetadataId` int NOT NULL AUTO_INCREMENT,
  `TrackId` int NOT NULL,
  `Plays` int NOT NULL,
  `Likes` int NOT NULL,
  `CommentsCount` int NOT NULL,
  PRIMARY KEY (`MetadataId`),
  UNIQUE KEY `IX_TrackMetadatas_TrackId` (`TrackId`),
  CONSTRAINT `FK_TrackMetadatas_Tracks_TrackId` FOREIGN KEY (`TrackId`) REFERENCES `tracks` (`TrackId`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `trackmetadatas`
--

LOCK TABLES `trackmetadatas` WRITE;
/*!40000 ALTER TABLE `trackmetadatas` DISABLE KEYS */;
/*!40000 ALTER TABLE `trackmetadatas` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `tracks`
--

DROP TABLE IF EXISTS `tracks`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `tracks` (
  `TrackId` int NOT NULL AUTO_INCREMENT,
  `Title` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Genre` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Tags` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Description` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `FilePath` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Duration` time(6) NOT NULL,
  `UploadDate` datetime(6) NOT NULL,
  `ArtistId` int NOT NULL,
  `UserId` int DEFAULT NULL,
  PRIMARY KEY (`TrackId`),
  KEY `IX_Tracks_ArtistId` (`ArtistId`),
  KEY `IX_Tracks_UserId` (`UserId`),
  CONSTRAINT `FK_Tracks_Artists_ArtistId` FOREIGN KEY (`ArtistId`) REFERENCES `artists` (`ArtistId`) ON DELETE CASCADE,
  CONSTRAINT `FK_Tracks_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `users` (`user_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tracks`
--

LOCK TABLES `tracks` WRITE;
/*!40000 ALTER TABLE `tracks` DISABLE KEYS */;
/*!40000 ALTER TABLE `tracks` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `users`
--

DROP TABLE IF EXISTS `users`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `users` (
  `user_id` int NOT NULL AUTO_INCREMENT,
  `email` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `password_hash` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `username` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `profile_image` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `biography` text CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `date_joined` datetime(6) NOT NULL,
  `oauth_provider` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `is_email_confirmed` tinyint(1) NOT NULL,
  `IsAdmin` tinyint(1) NOT NULL DEFAULT '0',
  PRIMARY KEY (`user_id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `users`
--

LOCK TABLES `users` WRITE;
/*!40000 ALTER TABLE `users` DISABLE KEYS */;
INSERT INTO `users` VALUES (1,'user1@example.com','hashed_password1','User1','image1.jpg','User 1 biography','2024-09-26 23:53:58.000000','provider1',1,0),(2,'user2@example.com','hashed_password2','User2','image2.jpg','User 2 biography','2024-09-26 23:53:58.000000','provider2',0,0),(3,'user3@example.com','hashed_password3','User3','image3.jpg','User 3 biography','2024-09-26 23:53:58.000000','provider3',1,0);
/*!40000 ALTER TABLE `users` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2024-09-28 22:06:45
