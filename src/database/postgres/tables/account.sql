CREATE TABLE public.account (
	id                    VARCHAR(36) NOT NULL PRIMARY KEY,
	tokenHash             VARCHAR(64) NULL,
	certificateThumbprint VARCHAR(40) NULL
);