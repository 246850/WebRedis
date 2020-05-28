-- --------------------------------------------------------
-- 主机:                           101.132.140.8
-- 服务器版本:                        5.6.39-log - MySQL Community Server (GPL)
-- 服务器OS:                        Win64
-- HeidiSQL 版本:                  10.2.0.5599
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;


-- Dumping database structure for webredis
DROP DATABASE IF EXISTS `webredis`;
CREATE DATABASE IF NOT EXISTS `webredis` /*!40100 DEFAULT CHARACTER SET utf8 */;
USE `webredis`;

-- Dumping structure for table webredis.connection_config
DROP TABLE IF EXISTS `connection_config`;
CREATE TABLE IF NOT EXISTS `connection_config` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(100) NOT NULL COMMENT '名称',
  `IpAddress` varchar(255) NOT NULL COMMENT '连接IP',
  `Port` int(11) NOT NULL COMMENT '端口,6379',
  `Password` varchar(50) DEFAULT NULL COMMENT '连接密码',
  `Description` varchar(300) DEFAULT NULL COMMENT '描述',
  `CreateTime` datetime NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8 COMMENT='Redis连接表';
