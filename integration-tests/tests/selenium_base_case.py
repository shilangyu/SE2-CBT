import os
import random
import unittest
from functools import cache
from unittest import TestCase
from uuid import uuid4

from selenium import webdriver
from selenium.webdriver.chrome.options import Options
from selenium.webdriver.chrome.service import Service
from selenium.webdriver.common.by import By
from selenium.webdriver.remote.webelement import WebElement
from webdriver_manager.chrome import ChromeDriverManager


@cache
def _get_chromedriver_path() -> str:
    return ChromeDriverManager().install()


class SeleniumBaseCase(TestCase):
    driver: webdriver.Chrome

    email: str
    password: str

    def setUp(self) -> None:
        assert 'TEST_URL' in os.environ, 'The required `TEST_URL` environment variable is missing'

        super().setUp()

        self.password = str(uuid4())
        self.email = f'{self.password}@integration.test'

        opts = Options()

        # reduce resource usage
        opts.add_argument('--incognito')
        opts.add_argument('--disable-remote-fonts')

        opts.add_argument('--num-raster-threads=1')
        opts.add_argument('--enable-low-end-device-mode')

        # run in headless mode by default
        opts.headless = not os.getenv('SELENIUM_SHOW')

        # consider page loaded when interactive (speeds up execution)
        opts.page_load_strategy = 'eager'

        self.driver = webdriver.Chrome(service=Service(_get_chromedriver_path()), options=opts)

        timeout = os.getenv('SELENIUM_TIMEOUT', 10)
        self.driver.implicitly_wait(timeout)
        self.driver.set_page_load_timeout(timeout)

        self.driver.get(os.environ['TEST_URL'])

        # create a new account
        self._find_test_element('login-create-account-button').click()
        self._find_test_element('register-email-input').send_keys(self.email)
        self._find_test_element('register-password-input').send_keys(self.password)
        self._find_test_element('register-age-input').send_keys(random.randint(30, 60))
        self._find_test_element('register-gender-input').send_keys('genderless')
        self._find_test_element('register-create-account-button').click()

    def tearDown(self) -> None:
        self.driver.close()

        super().tearDown()

    def _find_test_element(self, test_id: str) -> WebElement:
        return self.driver.find_element(By.CSS_SELECTOR, f'[data-testid={test_id}]')

    def testLogOutAndIn(self):
        self._find_test_element('navbar-logout-button').click()
        self._find_test_element('login-email-input').send_keys(self.email)
        self._find_test_element('login-password-input').send_keys(self.password)
        self._find_test_element('login-login-button').click()

        # check existence of the profile button (successful login)
        self._find_test_element('navbar-profile-button')

    # def testLogOutAndWrongPassword(self):
    #     self._find_test_element('navbar-logout-button').click()
    #     self._find_test_element('login-email-input').send_keys(self.email)
    #     self._find_test_element('login-password-input').send_keys(self.password + 'A')
    #     self._find_test_element('login-login-button').click()
    #
    #     # TODO: implement data-testid on "wrong credentials" error text
