CREATE EXTENSION if not exists "uuid-ossp";
CREATE EXTENSION if not exists pgcrypto;

CREATE TABLE IF NOT EXISTS app_user (
	id uuid, 
	first_name varchar(50), 
	last_name varchar(50), 
	age int,
	biography varchar(1000),
	city varchar(30),
	password_hash varchar(100)
);