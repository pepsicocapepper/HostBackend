-- Created by eric on 3/5/26 - 12:19:49
INSERT INTO branch(id, address_line_1, locality, administrative_area, country)
VALUES ('429ae922-83c0-4cda-8d31-256d842755f8', 'Priv. 18 de Octubre 2216', 'Nuevo Laredo', 'Tamaulipas', 'MX');

INSERT INTO host_user(name, surname, pin, branch_id,job_title,staffing_id)
VALUES ('Eric', 'Fernandez', '1234', '429ae922-83c0-4cda-8d31-256d842755f8','IT','0028b630-d07a-4b61-b6c6-0532c92f16f2')