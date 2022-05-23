import os
import unittest

from dotenv import load_dotenv

if __name__ == '__main__':
    load_dotenv()

    # TODO: implement unittest-parallel
    loader = unittest.TestLoader()
    suite = loader.discover('tests', pattern='*.py')
    result = unittest.TextTestRunner(verbosity=2, failfast=bool(os.getenv('TEST_FAILFAST', 0))).run(suite)

    if result.errors:
        exit(-1)
