-- Created by eric on 4/18/26 - 22:08:54
CREATE TABLE host_user_quickbooks_credentials
(
    user_id       UUID PRIMARY KEY NOT NULL,
    access_token  TEXT             NOT NULL,
    refresh_token TEXT             NOT NULL,
    FOREIGN KEY (user_id) REFERENCES host_user (id)
)