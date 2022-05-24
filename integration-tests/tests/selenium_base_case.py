import logging
import os
import random
from functools import cache
from unittest import TestCase
from uuid import uuid4

from dotenv import load_dotenv
from selenium import webdriver
from selenium.common.exceptions import NoSuchElementException, StaleElementReferenceException
from selenium.webdriver.chrome.options import Options
from selenium.webdriver.chrome.service import Service
from selenium.webdriver.chrome.webdriver import WebDriver
from selenium.webdriver.common.by import By
from selenium.webdriver.remote.webelement import WebElement
from selenium.webdriver.support.wait import WebDriverWait
from webdriver_manager.chrome import ChromeDriverManager
from webdriver_manager.utils import ChromeType


@cache
def _get_chromedriver_path() -> str:
    return ChromeDriverManager(chrome_type=ChromeType.CHROMIUM, log_level=logging.WARN).install()


class SeleniumBaseCase(TestCase):
    driver: webdriver.Chrome
    driver_wait: WebDriverWait

    email: str
    password: str

    admin_email: str = os.getenv('ADMIN_EMAIL', 'admin@admin.com')
    admin_password: str = os.getenv('ADMIN_PASSWORD', 'adminadmin')

    @staticmethod
    def _random_email_password() -> [str, str]:
        password = str(uuid4())
        return f'{password}@integration.invalid', password

    def __init__(self, *args, **kwargs):
        load_dotenv()
        super().__init__(*args, **kwargs)

    def setUp(self) -> None:
        assert 'TEST_URL' in os.environ, 'The required `TEST_URL` environment variable is missing'
        super().setUp()

        self.email, self.password = SeleniumBaseCase._random_email_password()

        opts = Options()

        # reduce resource usage
        opts.add_argument('--incognito')
        opts.add_argument('--disable-remote-fonts')

        opts.add_argument('--num-raster-threads=1')
        opts.add_argument('--enable-low-end-device-mode')

        # run in headless mode by default
        opts.headless = not os.getenv('SELENIUM_SHOW')

        self.driver = webdriver.Chrome(service=Service(_get_chromedriver_path()), options=opts)

        timeout = os.getenv('SELENIUM_TIMEOUT', 10)
        self.driver.implicitly_wait(timeout)
        self.driver.set_page_load_timeout(timeout)
        self.driver_wait = WebDriverWait(self.driver, timeout,
                                         ignored_exceptions=(NoSuchElementException, StaleElementReferenceException))

        self.driver.get(os.environ['TEST_URL'])

        # create a new account
        self._find_test_element('login-create-account-button').click()
        self._find_test_element('register-email-input').send_keys(self.email)
        self._find_test_element('register-password-input').send_keys(self.password)
        self._find_test_element('register-age-input').send_keys(random.randint(30, 60))
        self._find_test_element('register-gender-input').find_element(By.XPATH, '..').click()
        self._find_test_element('register-gender-input-item').click()
        self._find_test_element('register-create-account-button').click()

    def tearDown(self) -> None:
        self.driver.close()
        super().tearDown()

    def _find_test_elements(self, test_id: str, origin: WebElement | WebDriver = None) -> list[WebElement]:
        if origin is None:
            origin = self.driver

        return origin.find_elements(By.CSS_SELECTOR, f'[data-testid={test_id}]')

    def _find_test_element(self, test_id: str, origin: WebElement | WebDriver = None) -> WebElement:
        return self._find_test_elements(test_id, origin)[0]

    def _logout_login(self, *, admin: bool = False):
        self._find_test_element('navbar-logout-button').click()
        self._find_test_element('login-email-input').send_keys(self.admin_email if admin else self.email)
        self._find_test_element('login-password-input').send_keys(self.admin_password if admin else self.password)
        self._find_test_element('login-login-button').click()

    def testLogin(self):
        self._logout_login()

        # check existence of the profile button (successful login)
        self._find_test_element('navbar-profile-button')

    # def testLoginWrongPassword(self):
    #     self._find_test_element('navbar-logout-button').click()
    #     self._find_test_element('login-email-input').send_keys(self.email)
    #     self._find_test_element('login-password-input').send_keys(self.password + 'A')
    #     self._find_test_element('login-login-button').click()
    #
    #     # TODO: implement data-testid on "wrong credentials" error text

    def testAdminLogin(self):
        self._logout_login(admin=True)

        # check existence (successful login)
        self._find_test_element('dashboard-user-management')

    def testAdminUserList(self):
        self._logout_login(admin=True)
        self._find_test_element('dashboard-user-management').click()

        self.assertIn(self.email, (el.text for el in self._find_test_elements('admin-user-list-login')))

    def testAdminUserEditLogin(self):
        self._logout_login(admin=True)
        self._find_test_element('dashboard-user-management').click()

        entry = next(el for el in self._find_test_elements('admin-user-list-login') if el.text == self.email)
        entry = entry.find_element(By.XPATH, '..')

        self._find_test_element('admin-user-list-edit-button', origin=entry).click()

        new_email, _ = SeleniumBaseCase._random_email_password()

        self._find_test_element('admin-user-edit-login-input').clear()
        self._find_test_element('admin-user-edit-login-input').send_keys(new_email)
        self._find_test_element('admin-user-edit-save').click()

        self.driver_wait.until(
            lambda _: self._find_test_element('admin-user-list-login', origin=entry).text == new_email)

    def testAdminUserEditCancel(self):
        self._logout_login(admin=True)
        self._find_test_element('dashboard-user-management').click()

        entry = next(el for el in self._find_test_elements('admin-user-list-login') if el.text == self.email)
        entry = entry.find_element(By.XPATH, '..')

        self._find_test_element('admin-user-list-edit-button', origin=entry).click()
        self.assertEqual(self._find_test_element('admin-user-edit-login-input').get_attribute('value'), self.email)

        self._find_test_element('admin-user-edit-login-input').clear()
        self._find_test_element('admin-user-edit-login-input').send_keys('something@else.invalid')
        self._find_test_element('admin-user-edit-cancel').click()

        self._find_test_element('admin-user-list-edit-button', origin=entry).click()
        self.assertEqual(self._find_test_element('admin-user-edit-login-input').get_attribute('value'), self.email)

    def testAdminUserBan(self):
        self._logout_login(admin=True)
        self._find_test_element('dashboard-user-management').click()

        entry = next(el for el in self._find_test_elements('admin-user-list-login') if el.text == self.email)
        entry = entry.find_element(By.XPATH, '..')

        self.assertEqual(self._find_test_element('admin-user-list-banned', origin=entry).text, 'no')

        self._find_test_element('admin-user-list-ban-button', origin=entry).click()

        self.driver_wait.until(
            lambda _: self._find_test_element('admin-user-list-banned', origin=entry).text == 'yes')

    def testAdminUserDelete(self):
        self._logout_login(admin=True)
        self._find_test_element('dashboard-user-management').click()

        entry = next(el for el in self._find_test_elements('admin-user-list-login') if el.text == self.email)
        entry = entry.find_element(By.XPATH, '..')

        self._find_test_element('admin-user-list-delete-button', origin=entry).click()

        self.driver_wait.until(
            lambda _: self.email not in (el.text for el in self._find_test_elements('admin-user-list-login')))
