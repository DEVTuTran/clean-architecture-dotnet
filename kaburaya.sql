SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION';

-- -----------------------------------------------------
-- Schema kaburaya
-- -----------------------------------------------------
CREATE SCHEMA IF NOT EXISTS `kaburaya` 
DEFAULT CHARACTER SET utf8mb4 
COLLATE utf8mb4_unicode_ci
COMMENT 'Database hệ thống Kaburaya';

USE `kaburaya`;

-- -----------------------------------------------------
-- Table `kaburaya`.`organization_status`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `kaburaya`.`organization_status` (
  `id` TINYINT UNSIGNED NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(50) NOT NULL,
  `description` VARCHAR(255) NULL,
  `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  UNIQUE INDEX `uq_organization_status_name` (`name` ASC))
ENGINE = InnoDB
COMMENT = 'Trạng thái tổ chức (active, inactive, suspended)';

-- -----------------------------------------------------
-- Table `kaburaya`.`payment_fee`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `kaburaya`.`payment_fee` (
  `id` TINYINT UNSIGNED NOT NULL AUTO_INCREMENT,
  `fee` DECIMAL(10,2) UNSIGNED NOT NULL DEFAULT 10000.00,
  `description` VARCHAR(255) NULL,
  `is_active` TINYINT(1) NOT NULL DEFAULT 1,
  `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`))
ENGINE = InnoDB
COMMENT = 'Cấu hình phí thanh toán cho các tổ chức';

-- -----------------------------------------------------
-- Table `kaburaya`.`organization`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `kaburaya`.`organization` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(100) NOT NULL,
  `code` VARCHAR(20) NOT NULL,
  `status_id` TINYINT UNSIGNED NOT NULL DEFAULT 2,
  `max_payment_users` SMALLINT UNSIGNED NOT NULL DEFAULT 0,
  `payment_fee_id` TINYINT UNSIGNED NOT NULL DEFAULT 1,
  `use_ip_whitelist` TINYINT(1) NOT NULL DEFAULT 0,
  `is_deleted` TINYINT(1) NOT NULL DEFAULT 0,
  `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `deleted_at` TIMESTAMP NULL,
  PRIMARY KEY (`id`),
  UNIQUE INDEX `uq_organization_code` (`code` ASC),
  INDEX `idx_organization_status` (`status_id` ASC),
  INDEX `idx_organization_payment_fee` (`payment_fee_id` ASC),
  CONSTRAINT `fk_organization_status`
    FOREIGN KEY (`status_id`)
    REFERENCES `kaburaya`.`organization_status` (`id`)
    ON DELETE RESTRICT
    ON UPDATE CASCADE,
  CONSTRAINT `fk_organization_payment_fee`
    FOREIGN KEY (`payment_fee_id`)
    REFERENCES `kaburaya`.`payment_fee` (`id`)
    ON DELETE RESTRICT
    ON UPDATE CASCADE)
ENGINE = InnoDB
COMMENT = 'Thông tin các tổ chức sử dụng hệ thống';

-- -----------------------------------------------------
-- Table `kaburaya`.`buyer`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `kaburaya`.`buyer` (
  `organization_id` INT UNSIGNED NOT NULL,
  `family_name` VARCHAR(50) NOT NULL,
  `given_name` VARCHAR(50) NOT NULL,
  `family_name_kana` VARCHAR(100) NOT NULL,
  `given_name_kana` VARCHAR(100) NOT NULL,
  `zip_code` CHAR(7) NOT NULL,
  `prefecture` VARCHAR(20) NOT NULL,
  `city` VARCHAR(50) NOT NULL,
  `address` VARCHAR(100) NOT NULL,
  `company_name` VARCHAR(100) NOT NULL,
  `department` VARCHAR(50) NULL,
  `phone` VARCHAR(15) NOT NULL,
  `email` VARCHAR(255) NOT NULL,
  `president_family_name` VARCHAR(50) NOT NULL,
  `president_given_name` VARCHAR(50) NOT NULL,
  `president_family_name_kana` VARCHAR(100) NOT NULL,
  `president_given_name_kana` VARCHAR(100) NOT NULL,
  `birthday` DATE NULL,
  `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`organization_id`),
  INDEX `idx_buyer_email` (`email` ASC),
  INDEX `idx_buyer_phone` (`phone` ASC),
  CONSTRAINT `fk_buyer_organization`
    FOREIGN KEY (`organization_id`)
    REFERENCES `kaburaya`.`organization` (`id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE)
ENGINE = InnoDB
COMMENT = 'Thông tin người mua/đại diện tổ chức';

-- -----------------------------------------------------
-- Table `kaburaya`.`data_status`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `kaburaya`.`data_status` (
  `id` TINYINT UNSIGNED NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(50) NOT NULL,
  `description` VARCHAR(255) NULL,
  `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  UNIQUE INDEX `uq_data_status_name` (`name` ASC))
ENGINE = InnoDB
COMMENT = 'Trạng thái dữ liệu khách hàng';

-- -----------------------------------------------------
-- Table `kaburaya`.`client_data`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `kaburaya`.`client_data` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `code` VARCHAR(20) NOT NULL,
  `name` VARCHAR(100) NOT NULL,
  `description` TEXT NULL,
  `status_id` TINYINT UNSIGNED NOT NULL DEFAULT 2,
  `is_first_data` TINYINT(1) NOT NULL DEFAULT 0,
  `display_name` VARCHAR(100) NOT NULL DEFAULT '',
  `s3_key` VARCHAR(50) NOT NULL DEFAULT '',
  `is_favorite` TINYINT(1) NOT NULL DEFAULT 0,
  `file_size` INT UNSIGNED NULL COMMENT 'Kích thước file (bytes)',
  `file_type` VARCHAR(50) NULL COMMENT 'Loại file (csv, excel, etc.)',
  `last_updated_by` VARCHAR(100) NOT NULL,
  `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  UNIQUE INDEX `uq_client_data_code` (`code` ASC),
  INDEX `idx_client_data_status` (`status_id` ASC),
  FULLTEXT INDEX `ft_client_data_name` (`name`, `description`),
  CONSTRAINT `fk_client_data_status`
    FOREIGN KEY (`status_id`)
    REFERENCES `kaburaya`.`data_status` (`id`)
    ON DELETE RESTRICT
    ON UPDATE CASCADE)
ENGINE = InnoDB
COMMENT = 'Dữ liệu khách hàng';

-- -----------------------------------------------------
-- Table `kaburaya`.`client`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `kaburaya`.`client` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `code` VARCHAR(20) NOT NULL,
  `name` VARCHAR(100) NOT NULL,
  `description` TEXT NULL,
  `last_data_name` VARCHAR(100) NOT NULL DEFAULT '',
  `is_favorite` TINYINT(1) NOT NULL DEFAULT 0,
  `last_updated_by` VARCHAR(100) NOT NULL,
  `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  UNIQUE INDEX `uq_client_code` (`code` ASC),
  FULLTEXT INDEX `ft_client_name_desc` (`name`, `description`))
ENGINE = InnoDB
COMMENT = 'Thông tin khách hàng';

-- -----------------------------------------------------
-- Table `kaburaya`.`payment_plan`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `kaburaya`.`payment_plan` (
  `id` TINYINT UNSIGNED NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(50) NOT NULL,
  `description` VARCHAR(255) NULL,
  `price_monthly` DECIMAL(10,2) UNSIGNED NOT NULL,
  `price_yearly` DECIMAL(10,2) UNSIGNED NULL,
  `max_users` SMALLINT UNSIGNED NULL,
  `max_clients` INT UNSIGNED NULL,
  `is_active` TINYINT(1) NOT NULL DEFAULT 1,
  `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  UNIQUE INDEX `uq_payment_plan_name` (`name` ASC))
ENGINE = InnoDB
COMMENT = 'Các gói thanh toán dịch vụ';

-- -----------------------------------------------------
-- Table `kaburaya`.`role`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `kaburaya`.`role` (
  `id` TINYINT UNSIGNED NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(50) NOT NULL,
  `description` VARCHAR(255) NULL,
  `permissions` JSON NULL COMMENT 'Danh sách quyền dạng JSON',
  `is_system_role` TINYINT(1) NOT NULL DEFAULT 0,
  `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  UNIQUE INDEX `uq_role_name` (`name` ASC))
ENGINE = InnoDB
COMMENT = 'Vai trò và phân quyền người dùng';

-- -----------------------------------------------------
-- Table `kaburaya`.`user`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `kaburaya`.`user` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `uid` VARCHAR(30) NOT NULL,
  `payment_plan_id` TINYINT UNSIGNED NOT NULL DEFAULT 1,
  `name` VARCHAR(100) NULL,
  `email` VARCHAR(255) NOT NULL,
  `email_verified_at` TIMESTAMP NULL,
  `password_hash` VARCHAR(255) NULL,
  `role_id` TINYINT UNSIGNED NOT NULL DEFAULT 1,
  `is_disabled` TINYINT(1) NOT NULL DEFAULT 0,
  `clients_count` INT UNSIGNED NOT NULL DEFAULT 0,
  `is_owner` TINYINT(1) NOT NULL DEFAULT 0,
  `last_login_at` TIMESTAMP NULL,
  `last_login_ip` VARCHAR(45) NULL,
  `must_change_password` TINYINT(1) NOT NULL DEFAULT 0,
  `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  UNIQUE INDEX `uq_user_uid` (`uid` ASC),
  UNIQUE INDEX `uq_user_email` (`email` ASC),
  INDEX `idx_user_role` (`role_id` ASC),
  INDEX `idx_user_payment_plan` (`payment_plan_id` ASC),
  CONSTRAINT `fk_user_payment_plan`
    FOREIGN KEY (`payment_plan_id`)
    REFERENCES `kaburaya`.`payment_plan` (`id`)
    ON DELETE RESTRICT
    ON UPDATE CASCADE,
  CONSTRAINT `fk_user_role`
    FOREIGN KEY (`role_id`)
    REFERENCES `kaburaya`.`role` (`id`)
    ON DELETE RESTRICT
    ON UPDATE CASCADE)
ENGINE = InnoDB
COMMENT = 'Người dùng hệ thống';

-- -----------------------------------------------------
-- Table `kaburaya`.`client_data_mapping`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `kaburaya`.`client_data_mapping` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `client_id` INT UNSIGNED NOT NULL,
  `client_data_id` INT UNSIGNED NOT NULL,
  `user_id` INT UNSIGNED NOT NULL,
  `organization_id` INT UNSIGNED NOT NULL,
  `access_level` TINYINT UNSIGNED NOT NULL DEFAULT 1 COMMENT '1: Read, 2: Write, 3: Owner',
  `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  UNIQUE INDEX `uq_client_data_mapping` (`client_id` ASC, `client_data_id` ASC, `user_id` ASC),
  INDEX `idx_client_data_mapping_data` (`client_data_id` ASC),
  INDEX `idx_client_data_mapping_user` (`user_id` ASC),
  INDEX `idx_client_data_mapping_org` (`organization_id` ASC),
  CONSTRAINT `fk_client_data_mapping_client`
    FOREIGN KEY (`client_id`)
    REFERENCES `kaburaya`.`client` (`id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE,
  CONSTRAINT `fk_client_data_mapping_data`
    FOREIGN KEY (`client_data_id`)
    REFERENCES `kaburaya`.`client_data` (`id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE,
  CONSTRAINT `fk_client_data_mapping_user`
    FOREIGN KEY (`user_id`)
    REFERENCES `kaburaya`.`user` (`id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE,
  CONSTRAINT `fk_client_data_mapping_org`
    FOREIGN KEY (`organization_id`)
    REFERENCES `kaburaya`.`organization` (`id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE)
ENGINE = InnoDB
COMMENT = 'Liên kết dữ liệu khách hàng với người dùng và tổ chức';

-- -----------------------------------------------------
-- Table `kaburaya`.`max_user_record`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `kaburaya`.`max_user_record` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `organization_id` INT UNSIGNED NOT NULL,
  `max_payment_users` SMALLINT UNSIGNED NOT NULL,
  `changed_by` INT UNSIGNED NULL COMMENT 'User ID thực hiện thay đổi',
  `reason` VARCHAR(255) NULL COMMENT 'Lý do thay đổi',
  `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  INDEX `idx_max_user_record_org` (`organization_id` ASC),
  INDEX `idx_max_user_record_changed_by` (`changed_by` ASC),
  CONSTRAINT `fk_max_user_record_org`
    FOREIGN KEY (`organization_id`)
    REFERENCES `kaburaya`.`organization` (`id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE,
  CONSTRAINT `fk_max_user_record_user`
    FOREIGN KEY (`changed_by`)
    REFERENCES `kaburaya`.`user` (`id`)
    ON DELETE SET NULL
    ON UPDATE CASCADE)
ENGINE = InnoDB
COMMENT = 'Lịch sử thay đổi số lượng user tối đa của tổ chức';

-- -----------------------------------------------------
-- Table `kaburaya`.`organization_ip_whitelist`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `kaburaya`.`organization_ip_whitelist` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `organization_id` INT UNSIGNED NOT NULL,
  `ip_address` VARCHAR(45) NOT NULL COMMENT 'IPv4 hoặc IPv6',
  `name` VARCHAR(100) NOT NULL COMMENT 'Mô tả IP',
  `is_active` TINYINT(1) NOT NULL DEFAULT 1,
  `created_by` INT UNSIGNED NULL,
  `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  UNIQUE INDEX `uq_org_ip_whitelist` (`organization_id` ASC, `ip_address` ASC),
  INDEX `idx_org_ip_whitelist_created_by` (`created_by` ASC),
  CONSTRAINT `fk_org_ip_whitelist_org`
    FOREIGN KEY (`organization_id`)
    REFERENCES `kaburaya`.`organization` (`id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE,
  CONSTRAINT `fk_org_ip_whitelist_user`
    FOREIGN KEY (`created_by`)
    REFERENCES `kaburaya`.`user` (`id`)
    ON DELETE SET NULL
    ON UPDATE CASCADE)
ENGINE = InnoDB
COMMENT = 'Danh sách IP được phép truy cập cho tổ chức';

-- -----------------------------------------------------
-- Table `kaburaya`.`organization_user`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `kaburaya`.`organization_user` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `user_id` INT UNSIGNED NOT NULL,
  `organization_id` INT UNSIGNED NOT NULL,
  `role_id` TINYINT UNSIGNED NULL,
  `joined_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `invited_by` INT UNSIGNED NULL,
  `is_active` TINYINT(1) NOT NULL DEFAULT 1,
  `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  UNIQUE INDEX `uq_org_user` (`user_id` ASC, `organization_id` ASC),
  INDEX `idx_org_user_org` (`organization_id` ASC),
  INDEX `idx_org_user_role` (`role_id` ASC),
  CONSTRAINT `fk_org_user_user`
    FOREIGN KEY (`user_id`)
    REFERENCES `kaburaya`.`user` (`id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE,
  CONSTRAINT `fk_org_user_org`
    FOREIGN KEY (`organization_id`)
    REFERENCES `kaburaya`.`organization` (`id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE,
  CONSTRAINT `fk_org_user_role`
    FOREIGN KEY (`role_id`)
    REFERENCES `kaburaya`.`role` (`id`)
    ON DELETE SET NULL
    ON UPDATE CASCADE)
ENGINE = InnoDB
COMMENT = 'Liên kết nhiều-nhiều giữa người dùng và tổ chức';

-- -----------------------------------------------------
-- Table `kaburaya`.`tax_type`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `kaburaya`.`tax_type` (
  `id` TINYINT UNSIGNED NOT NULL AUTO_INCREMENT,
  `rate` DECIMAL(5,2) UNSIGNED NOT NULL COMMENT 'Phần trăm thuế (8%, 10%)',
  `name` VARCHAR(50) NOT NULL,
  `code` VARCHAR(10) NOT NULL COMMENT 'Mã thuế',
  `is_active` TINYINT(1) NOT NULL DEFAULT 1,
  `effective_from` DATE NOT NULL,
  `effective_to` DATE NULL,
  `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  UNIQUE INDEX `uq_tax_type_code` (`code` ASC))
ENGINE = InnoDB
COMMENT = 'Loại thuế suất';

-- -----------------------------------------------------
-- Table `kaburaya`.`payment_collection`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `kaburaya`.`payment_collection` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `organization_id` INT UNSIGNED NOT NULL,
  `transaction_id` VARCHAR(50) NOT NULL COMMENT 'ID giao dịch từ gateway',
  `order_date` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `billed_amount` DECIMAL(12,2) UNSIGNED NOT NULL,
  `tax_id` TINYINT UNSIGNED NOT NULL,
  `tax_amount` DECIMAL(12,2) UNSIGNED NOT NULL,
  `total_amount` DECIMAL(12,2) UNSIGNED NOT NULL,
  `product_name` VARCHAR(100) NOT NULL,
  `quantity` SMALLINT UNSIGNED NOT NULL DEFAULT 1,
  `unit_price` DECIMAL(12,2) UNSIGNED NOT NULL,
  `fee_tax_type` TINYINT UNSIGNED NOT NULL,
  `payment_method` VARCHAR(20) NOT NULL COMMENT 'credit_card, bank_transfer, etc.',
  `payment_status` VARCHAR(20) NOT NULL COMMENT 'pending, completed, failed',
  `details` JSON NULL COMMENT 'Chi tiết giao dịch dạng JSON',
  `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  UNIQUE INDEX `uq_payment_collection_transaction` (`transaction_id` ASC),
  INDEX `idx_payment_collection_org` (`organization_id` ASC),
  INDEX `idx_payment_collection_tax` (`tax_id` ASC),
  INDEX `idx_payment_collection_date` (`order_date` ASC),
  CONSTRAINT `fk_payment_collection_org`
    FOREIGN KEY (`organization_id`)
    REFERENCES `kaburaya`.`organization` (`id`)
    ON DELETE RESTRICT
    ON UPDATE CASCADE,
  CONSTRAINT `fk_payment_collection_tax`
    FOREIGN KEY (`tax_id`)
    REFERENCES `kaburaya`.`tax_type` (`id`)
    ON DELETE RESTRICT
    ON UPDATE CASCADE)
ENGINE = InnoDB
COMMENT = 'Lịch sử thu phí thanh toán';

-- -----------------------------------------------------
-- Table `kaburaya`.`user_session`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `kaburaya`.`user_session` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `user_id` INT UNSIGNED NOT NULL,
  `session_token` VARCHAR(100) NOT NULL,
  `refresh_token` VARCHAR(100) NULL,
  `device_id` VARCHAR(100) NOT NULL,
  `device_name` VARCHAR(100) NULL,
  `ip_address` VARCHAR(45) NOT NULL,
  `user_agent` TEXT NULL,
  `token_issued_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `token_expires_at` TIMESTAMP NOT NULL,
  `refresh_token_expires_at` TIMESTAMP NULL,
  `is_active` TINYINT(1) NOT NULL DEFAULT 1,
  `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  UNIQUE INDEX `uq_user_session_token` (`session_token` ASC),
  INDEX `idx_user_session_user` (`user_id` ASC),
  INDEX `idx_user_session_expiry` (`token_expires_at` ASC),
  CONSTRAINT `fk_user_session_user`
    FOREIGN KEY (`user_id`)
    REFERENCES `kaburaya`.`user` (`id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE)
ENGINE = InnoDB
COMMENT = 'Phiên đăng nhập người dùng';

-- -----------------------------------------------------
-- Table `kaburaya`.`user_client`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `kaburaya`.`user_client` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `client_id` INT UNSIGNED NOT NULL,
  `user_id` INT UNSIGNED NOT NULL,
  `organization_id` INT UNSIGNED NOT NULL,
  `access_level` TINYINT UNSIGNED NOT NULL DEFAULT 1 COMMENT '1: Read, 2: Write, 3: Owner',
  `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  UNIQUE INDEX `uq_user_client` (`client_id` ASC, `user_id` ASC),
  INDEX `idx_user_client_user` (`user_id` ASC),
  INDEX `idx_user_client_org` (`organization_id` ASC),
  CONSTRAINT `fk_user_client_client`
    FOREIGN KEY (`client_id`)
    REFERENCES `kaburaya`.`client` (`id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE,
  CONSTRAINT `fk_user_client_user`
    FOREIGN KEY (`user_id`)
    REFERENCES `kaburaya`.`user` (`id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE,
  CONSTRAINT `fk_user_client_org`
    FOREIGN KEY (`organization_id`)
    REFERENCES `kaburaya`.`organization` (`id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE)
ENGINE = InnoDB
COMMENT = 'Liên kết nhiều-nhiều giữa người dùng và khách hàng';

-- -----------------------------------------------------
-- Table `kaburaya`.`audit_log`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `kaburaya`.`audit_log` (
  `id` BIGINT UNSIGNED NOT NULL AUTO_INCREMENT,
  `user_id` INT UNSIGNED NULL,
  `action` VARCHAR(50) NOT NULL COMMENT 'create, update, delete, login, etc.',
  `entity_type` VARCHAR(50) NOT NULL COMMENT 'Tên bảng/entity',
  `entity_id` VARCHAR(50) NULL COMMENT 'ID bản ghi',
  `old_values` JSON NULL COMMENT 'Giá trị cũ (dạng JSON)',
  `new_values` JSON NULL COMMENT 'Giá trị mới (dạng JSON)',
  `ip_address` VARCHAR(45) NULL,
  `user_agent` TEXT NULL,
  `created_at` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  INDEX `idx_audit_log_user` (`user_id` ASC),
  INDEX `idx_audit_log_entity` (`entity_type` ASC, `entity_id` ASC),
  INDEX `idx_audit_log_date` (`created_at` ASC),
  CONSTRAINT `fk_audit_log_user`
    FOREIGN KEY (`user_id`)
    REFERENCES `kaburaya`.`user` (`id`)
    ON DELETE SET NULL
    ON UPDATE CASCADE)
ENGINE = InnoDB
COMMENT = 'Nhật ký hoạt động hệ thống';

-- -----------------------------------------------------
-- Table `kaburaya`.`__EFMigrationsHistory`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `kaburaya`.`__EFMigrationsHistory` (
  `MigrationId` VARCHAR(95) NOT NULL,
  `ProductVersion` VARCHAR(32) NOT NULL,
  `AppliedAt` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`MigrationId`),
  INDEX `idx_EFMigrationsHistory_date` (`AppliedAt` ASC))
ENGINE = InnoDB
COMMENT = 'Lịch sử migrations Entity Framework Core';

SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;