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

-- Dumping data for table webredis.connection_config: ~0 rows (大约)
DELETE FROM `connection_config`;
/*!40000 ALTER TABLE `connection_config` DISABLE KEYS */;
INSERT INTO `connection_config` (`Id`, `Name`, `IpAddress`, `Port`, `Password`, `Description`, `CreateTime`) VALUES
	(5, '172.16.10.246', '172.16.10.246', 6379, NULL, '172.16.10.246', '2020-05-28 16:32:08'),
	(6, '101.132.140.8	', '101.132.140.8', 6379, NULL, '101.132.140.8	', '2020-05-28 16:34:14');
/*!40000 ALTER TABLE `connection_config` ENABLE KEYS */;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
